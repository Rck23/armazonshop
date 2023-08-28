using AutoMapper;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.DeleteShoppingCartItem;

public class DeleteShoppingCartItemCommandHandle : IRequestHandler<DeleteShoppingCartItemCommand, ShoppingCartVm>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteShoppingCartItemCommandHandle(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ShoppingCartVm> Handle(DeleteShoppingCartItemCommand request, CancellationToken cancellationToken)
    {
        var shoppingCartItem = await _unitOfWork.Repository<ShoppingCartItem>().GetEntityAsync(

            x => x.Id == request.Id
        );

        await _unitOfWork.Repository<ShoppingCartItem>().DeleteAsync( shoppingCartItem );

        var includes = new List<Expression<Func<ShoppingCart, object>>>();
        includes.Add(p => p.ShoppingCartItems!.OrderBy( x => x.Producto ) );

        var shoppingCart = await _unitOfWork.Repository<ShoppingCart>().GetEntityAsync(
            x => x.ShoppingCartMasterId == shoppingCartItem.ShoppingCartMasterId,
            includes,
            true
            
        );

        return _mapper.Map<ShoppingCartVm>( shoppingCart );

    }
}
