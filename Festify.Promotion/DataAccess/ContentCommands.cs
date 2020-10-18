using Festify.Promotion.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Festify.Promotion.DataAccess
{
    public class ContentCommands
    {
        private readonly PromotionContext repository;

        public ContentCommands(PromotionContext repository)
        {
            this.repository = repository;
        }

        public async Task<string> SaveContent(byte[] binary)
        {
            var sha512 = HashAlgorithm.Create(HashAlgorithmName.SHA512.Name);
            var hash = Convert.ToBase64String(sha512.ComputeHash(binary));

            var exists = await repository.Content
                .Where(c => c.Hash == hash)
                .AnyAsync();

            if (!exists)
            {
                await repository.Content.AddAsync(new Content
                {
                    Hash = hash,
                    Binary = binary
                });
                await repository.SaveChangesAsync();
            }

            return hash;
        }
    }
}
