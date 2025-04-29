using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Navigation.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Application.Journy.Queries.GetJourny
{
    public record GetJourneysQuery(PaginationRequest PaginationRequest)
     : IQuery<GetJourneysResult>;
    public record GetJourneysResult(PaginatedResult<JourneyDto> Journeys);
}
