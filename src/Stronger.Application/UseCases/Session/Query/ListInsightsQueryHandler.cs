using System;
using System.Linq;
using System.Text.Json;
using MediatR;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.Session.Insight;
using Stronger.Domain.Common;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.Application.UseCases.Session.Query;

public class ListInsightsQueryHandler(
    IClaimsService claims,
    IRepositoryManager repository
) : IRequestHandler<ListInsightsQuery, Response<InsightsResponse>>
{
    async Task<Response<InsightsResponse>> IRequestHandler<ListInsightsQuery, Response<InsightsResponse>>.Handle(ListInsightsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ExerciseEntity> exerciseEntities = await repository.Exercises.ListAsync(null, cancellationToken);

        Dictionary<long, String> exerciseNameCache = new Dictionary<long, String>();

        foreach(var exercise in exerciseEntities)
        {
            exerciseNameCache[exercise.Id] = exercise.Name;
        }

        Guid userId = new Guid(claims.UserId);

        IEnumerable<SessionEntity> userSessions = await repository.Sessions.ListAsync(session => session.UserId == userId, cancellationToken);
        //userSessions = userSessions.OrderBy(u => u.CreatedAt);

        List<SessionData> userSessionData = new List<SessionData>();

        foreach(var userSession in userSessions)
        {
            var session = JsonSerializer.Deserialize<SessionData>(userSession.SessionData);

            if(session is null)
            {
                throw new FormatException($"Session with ID:{userSession.Id} has inccorrect JSON formatiing");
            }

            userSessionData.Add(session);
        }

        InsightsResponse insights = new();

        // Process sessions in chronological order (optional but useful for graphing)
        foreach (var session in userSessionData.OrderBy(s => s.Date))
        {
            foreach (var exercise in session.Exercises)
            {
                // Resolve name; fall back to id if missing in cache
                string exerciseName = exerciseNameCache.TryGetValue(exercise.Id, out var name)
                    ? name
                    : $"Exercise:{exercise.Id}";

                // Ignore null weights (e.g., cardio sets)
                var weights = exercise.Sets
                    .Select(s => s.Weight)
                    .Where(w => w.HasValue)
                    .Select(w => w.Value);

                if (!weights.Any())
                {
                    continue;
                }

                float maxWeight = weights.Max();

                if (!insights.Plots.TryGetValue(exerciseName, out var plots))
                {
                    plots = new List<InsightsResponse.SetDataPlot>();
                    insights.Plots[exerciseName] = plots;
                }

                plots.Add(new InsightsResponse.SetDataPlot
                {
                    Date = session.Date,
                    MaxWeight = maxWeight
                });
            }
        }

        // Optional: ensure each exercise plot list is sorted by date
        foreach (var kvp in insights.Plots)
        {
            kvp.Value.Sort((a, b) => a.Date.CompareTo(b.Date));
        }

        return new Response<InsightsResponse>
        {
            StatusCode = 200,
            Content = insights
        };
    }
}
