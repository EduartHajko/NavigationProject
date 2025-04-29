using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Navigation.Application.Data;
using Navigation.Application.Dtos;
using Navigation.Application.Extensions;


namespace Navigation.Application.Journy.Queries.GetJourny
{
    public class GetJourneysHandler(IApplicationDbContext dbContext)
      : IQueryHandler<GetJourneysQuery, GetJourneysResult>
    {
        public async Task<GetJourneysResult> Handle(GetJourneysQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.Journeys.LongCountAsync(cancellationToken);

            var journeys = await dbContext.Journeys
                .Include(j => j.SharedWithUsers)
                .Include(j => j.PublicShareLinks)
                .OrderBy(j => j.StartTime)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new GetJourneysResult(
                new PaginatedResult<JourneyDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    journeys.ToJourneyDtoList()
                ));
        }
    }
}
