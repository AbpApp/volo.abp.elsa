using System.Linq;
using Elsa.Models;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.WorkFlowManagement.EntityFrameworkCore
{
    public static class WorkFlowManagementDbContextModelBuilderExtensions
    {
        public static IQueryable<WorkflowDefinitionVersion> IncludeDetails(this IQueryable<WorkflowDefinitionVersion> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .AsSplitQuery()
                .Include(x => x.Activities)
                .Include(x => x.Connections);
        }
        public static IQueryable<WorkflowDefinitionVersion> WithVersion(
            this IQueryable<WorkflowDefinitionVersion> query,
            VersionOptions version)
        {

            if (version.IsDraft)
                query = query.Where(x => !x.IsPublished);
            else if (version.IsLatest)
                query = query.Where(x => x.IsLatest);
            else if (version.IsPublished)
                query = query.Where(x => x.IsPublished);
            else if (version.IsLatestOrPublished)
                query = query.Where(x => x.IsPublished || x.IsLatest);
            else if (version.AllVersions)
            {
                // Nothing to filter.
            }
            else if (version.Version > 0)
                query = query.Where(x => x.Version == version.Version);

            return query.OrderByDescending(x => x.Version);
        }
    }
}