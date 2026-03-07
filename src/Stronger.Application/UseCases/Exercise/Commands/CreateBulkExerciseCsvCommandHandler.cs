
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.Exercise;
using Stronger.Domain.Entities;
using Stronger.Domain.Enums;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Exercise.Commands;

public sealed class CreateBulkExerciseCsvCommandHandler(IRepositoryManager repository)
    : IRequestHandler<CreateBulkExerciseCsvCommand, Response<CreateExerciseResponse>>
{
    public async Task<Response<CreateExerciseResponse>> Handle(CreateBulkExerciseCsvCommand request, CancellationToken cancellationToken)
    {
    ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        const long maxBytes = 10 * 1024 * 1024; // 10MB
        if (request.File.Length > maxBytes)
        {
            return new Response<CreateExerciseResponse>
            {
                StatusCode = 400
            };
        }

        List<CsvExerciseRow> rows;
        await using (var stream = request.File.OpenReadStream())
        using (var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: false))
        {
            var csvText = await reader.ReadToEndAsync(cancellationToken);
            rows = ParseCsv(csvText);
        }

        if (rows.Count == 0)
        {
            return new Response<CreateExerciseResponse>
            {
                StatusCode = 400
            };
        }

        var exercises = new List<ExerciseEntity>(capacity: rows.Count);
        var errors = new List<string>();

        for (var i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            var rowNumber = i + 2; 

            if (string.IsNullOrWhiteSpace(row.Name))
            {
                errors.Add($"Row {rowNumber}: Name is required.");
                continue;
            }

            if (!Enum.TryParse<MuscleGroup>(row.PrimaryMuscleGroup, ignoreCase: true, out var primary))
            {
                errors.Add($"Row {rowNumber}: PrimaryMuscleGroup '{row.PrimaryMuscleGroup}' is invalid.");
                continue;
            }

            MuscleGroup? secondary = null;
            if (!string.IsNullOrWhiteSpace(row.SecondaryMuscleGroup))
            {
                if (Enum.TryParse<MuscleGroup>(row.SecondaryMuscleGroup, ignoreCase: true, out var secParsed))
                {
                    secondary = secParsed;
                }
                else
                {
                    errors.Add($"Row {rowNumber}: SecondaryMuscleGroup '{row.SecondaryMuscleGroup}' is invalid.");
                    continue;
                }
            }

            if (!Enum.TryParse<ExerciseType>(row.ExerciseType, ignoreCase: true, out var exerciseType))
            {
                errors.Add($"Row {rowNumber}: ExerciseType '{row.ExerciseType}' is invalid.");
                continue;
            }

            if (!Enum.TryParse<ForceType>(row.ForceType, ignoreCase: true, out var forceType))
            {
                errors.Add($"Row {rowNumber}: ForceType '{row.ForceType}' is invalid.");
                continue;
            }

            exercises.Add(new ExerciseEntity
            {
                Name = row.Name.Trim(),
                NameNormalized = row.Name.Trim().ToUpper(),
                Description = row.Description?.Trim() ?? string.Empty,
                ImagePath = row.ImagePath?.Trim() ?? string.Empty,
                PrimaryMuscleGroup = primary,
                SecondaryMuscleGroup = secondary,
                ExerciseType = exerciseType,
                ForceType = forceType
            });
        }

        if (errors.Count > 0)
        {
            return new Response<CreateExerciseResponse>
            {
                StatusCode = 400
            };
        }

        foreach(var exercise in exercises)
        {
            await repository.Exercises.AddAsync(exercise, cancellationToken);
        }
        
        await repository.SaveChangesAsync(cancellationToken);

        return new Response<CreateExerciseResponse>
        {
            StatusCode = 200
        };
    }

    private sealed record CsvExerciseRow(
        string Name,
        string? Description,
        string? ImagePath,
        string PrimaryMuscleGroup,
        string? SecondaryMuscleGroup,
        string ExerciseType,
        string ForceType
    );

    private static List<CsvExerciseRow> ParseCsv(string csvText)
    {
        var lines = SplitLines(csvText);
        if (lines.Count == 0) return new List<CsvExerciseRow>();

        var header = ParseCsvLine(lines[0]);
        var expected = new[]
        {
            "Name","Description","ImagePath","PrimaryMuscleGroup","SecondaryMuscleGroup","ExerciseType","ForceType"
        };

        if (header.Count < expected.Length || !expected.SequenceEqual(header.Take(expected.Length), StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidDataException(
                $"CSV header is invalid. Expected: {string.Join(",", expected)}");
        }

        var rows = new List<CsvExerciseRow>(capacity: Math.Max(0, lines.Count - 1));
        for (var i = 1; i < lines.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            var fields = ParseCsvLine(lines[i]);
            if (fields.Count == 0) continue;

            while (fields.Count < expected.Length) fields.Add(string.Empty);

            rows.Add(new CsvExerciseRow(
                Name: fields[0],
                Description: fields[1],
                ImagePath: fields[2],
                PrimaryMuscleGroup: fields[3],
                SecondaryMuscleGroup: fields[4],
                ExerciseType: fields[5],
                ForceType: fields[6]
            ));
        }

        return rows;
    }

    private static List<string> SplitLines(string text)
    {
        return text
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace("\r", "\n", StringComparison.Ordinal)
            .Split('\n', StringSplitOptions.None)
            .ToList();
    }

    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        var sb = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    sb.Append('"');
                    i++;
                    continue;
                }

                inQuotes = !inQuotes;
                continue;
            }

            if (c == ',' && !inQuotes)
            {
                result.Add(sb.ToString());
                sb.Clear();
                continue;
            }

            sb.Append(c);
        }

        result.Add(sb.ToString());
        return result;
    }
}
