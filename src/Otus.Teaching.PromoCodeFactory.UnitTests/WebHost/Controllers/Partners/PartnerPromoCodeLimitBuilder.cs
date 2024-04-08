using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class PartnerPromoCodeLimitBuilder
    {
        private Guid _id;
        private Guid _partnerId;
        private Partner _partner;
        private DateTime _createDate;
        private DateTime? _cancelDate;
        private DateTime _endDate;
        private int _limit;

        public PartnerPromoCodeLimitBuilder()
        {
            
        }

        public PartnerPromoCodeLimitBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public PartnerPromoCodeLimitBuilder WithPartnerId(Guid partnerId)
        {
            _partnerId = partnerId;
            return this;
        }

        public PartnerPromoCodeLimitBuilder WithPartner(Partner partner)
        {
            _partner = partner;
            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCreateDate(DateTime createDate)
        {
            _createDate = createDate;
            return this;
        }

        public PartnerPromoCodeLimitBuilder WithCancelDate(DateTime? cancelDate)
        {
            _cancelDate = cancelDate;
            return this;
        }

        public PartnerPromoCodeLimitBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;
            return this;
        }

        public PartnerPromoCodeLimitBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public PartnerPromoCodeLimit Build()
            => new()
            {
                Id = _id,
                PartnerId = _partnerId,
                Partner = _partner,
                CreateDate = _createDate,
                CancelDate = _cancelDate,
                EndDate = _endDate,
                Limit = _limit
            };
    }
}
