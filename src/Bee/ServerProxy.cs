﻿using System;
using System.Collections.Generic;
using System.Linq;
using Business;
using System.Threading.Tasks;

namespace Bee {
    class ServerProxy1 {
        public async static Task<IEnumerable<Category>> UpsertCategoriesAsync(IEnumerable<Category> categories) {
            return await ProxyHelper.PostAsync<IEnumerable<Category>>("Categories/Upsert", categories);
        }

        public async static Task UpsertProductsAsync(string categoryId, IEnumerable<Product> products) {
            await ProxyHelper.PostAsync<object>("CategoryProducts/Upsert", new {
                CategoryId = categoryId,
                Products = products
            });
        }

        public async static Task<IEnumerable<ProductSignature>> GetProductSignaturesByCategoryIdAsync(string categoryId) {
            return await ProxyHelper.GetAsync<IEnumerable<ProductSignature>>("Products/GetSignaturesByCategoryId?categoryId={0}", new[] { categoryId });
        }
    }
}