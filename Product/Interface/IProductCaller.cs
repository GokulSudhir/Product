using Product.Model;

namespace Product.Interface
{
    public interface IProductCaller
    {
        public Task<ApiResponse<IList<Products>>> GetAll();
        public Task<ApiResponse<Products>> Add(Products products);
        public Task<ApiResponse<Products>> Get(string product_code);
        public Task<ApiResponse<Products>> Edit(Products products);
        public Task<ApiResponse<Products>> Delete(string product_code);
    }
}
