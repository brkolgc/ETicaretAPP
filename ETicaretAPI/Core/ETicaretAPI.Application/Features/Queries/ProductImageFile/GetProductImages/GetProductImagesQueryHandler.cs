using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration;
        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            var baseUrl = _configuration["BaseStorageUrl"];

            var response = await _productReadRepository.Table
                .Where(p => p.Id == Guid.Parse(request.Id))
                .SelectMany(p => p.ProductImageFiles.Select(img => new GetProductImagesQueryResponse
                {
                    Path = baseUrl + "/" + img.Path,
                    FileName = img.FileName,
                    Id = img.Id
                }))
                .ToListAsync();

           return response;
        }
    }
}
