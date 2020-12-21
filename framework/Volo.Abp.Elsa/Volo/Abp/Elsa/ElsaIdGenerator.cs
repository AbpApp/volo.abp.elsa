using Elsa.Services;
using System;
using Volo.Abp.Guids;

namespace Volo.Abp.Elsa
{
    public class ElsaIdGenerator : IIdGenerator
    {
        private readonly IGuidGenerator _guidGenerator;
        public ElsaIdGenerator(IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }
        public string Generate()
        {
            return _guidGenerator.Create().ToString();
        }
    }
}
