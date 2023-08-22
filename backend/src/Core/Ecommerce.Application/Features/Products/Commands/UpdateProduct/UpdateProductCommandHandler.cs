
using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductVm> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productoToUpdate = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id); 
        if (productoToUpdate == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        // ACTUALIZAR LOS PRODUCTOS
        _mapper.Map(request, productoToUpdate, typeof(UpdateProductCommand), typeof(Product));

        await _unitOfWork.Repository<Product>().UpdateAsync(productoToUpdate);  


        // ACTUALIZAR LAS IMAGENES
        if((request.ImageUrls is not null) && request.ImageUrls.Count > 0)
        {
            var imagesToRemove = await _unitOfWork.Repository<Image>().GetAsync(
                x => x.ProductId == request.Id    
            );

            // CONJUNTO DE IMAGENES A ELIMINAR
            _unitOfWork.Repository<Image>().DeleteRange(imagesToRemove);

            // CONJUNTO DE IMAGENES A AGREGAR
            request.ImageUrls.Select(c => { c.ProductId = request.Id; return c; }).ToList();   
            var images = _mapper.Map<List<Image>>(request.ImageUrls);

            //ACTUALIZAR AL SERVIDOR (base de datos)
            _unitOfWork.Repository<Image>().AddRange(images);
            
            await _unitOfWork.Complete();
        }

        return _mapper.Map<ProductVm>(productoToUpdate); 
    }
}
