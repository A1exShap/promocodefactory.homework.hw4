using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class PartnerBuilder
    {
        private Guid _id;
        private string _name;
        private int _numberIssuedPromoCodes;
        private bool _isActive;
        private ICollection<PartnerPromoCodeLimit> _partnerLimits;

        public PartnerBuilder()
        {
            
        }

        public PartnerBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public PartnerBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PartnerBuilder WithNumberIssuedPromoCodes(int numberIssuedPromoCodes)
        {
            _numberIssuedPromoCodes = numberIssuedPromoCodes;
            return this;
        }

        public PartnerBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public PartnerBuilder WithPartnerLimits(ICollection<PartnerPromoCodeLimit> partnerLimits)
        {
            _partnerLimits = partnerLimits;
            return this;
        }

        public Partner Build()
            => new()
            {
                Id = _id,
                Name = _name,
                NumberIssuedPromoCodes= _numberIssuedPromoCodes,
                IsActive= _isActive,
                PartnerLimits= _partnerLimits
            };
    }
}
