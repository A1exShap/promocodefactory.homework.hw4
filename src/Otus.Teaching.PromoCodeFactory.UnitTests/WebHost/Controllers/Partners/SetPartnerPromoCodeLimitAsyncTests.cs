using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;

        public SetPartnerPromoCodeLimitAsyncTests() 
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            Partner partner = null;

            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            
            var partner = new PartnerBuilder().WithIsActive(false).Build();

            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_NumberIssuedPromoCodes_ResetNumberIfLimitIsNotOver()
        {
            // Arrange
            var partnerId = Guid.NewGuid();

            var limit = new PartnerPromoCodeLimitBuilder()
                .WithEndDate(DateTime.Now.AddDays(-1))
                .WithCancelDate(null)
                .Build();

            var partner = new PartnerBuilder()
                .WithNumberIssuedPromoCodes(10)
                .WithPartnerLimits(new List<PartnerPromoCodeLimit> { limit })
                .WithIsActive(true)
                .Build();

            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            partner.NumberIssuedPromoCodes.Should().Be(0);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_NumberIssuedPromoCodes_NoResetNumberIfLimitIsOver()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var numberIssuedPromoCodes = 10;

            var limit = new PartnerPromoCodeLimitBuilder().WithEndDate(DateTime.Now.AddDays(1)).Build();

            var partner = new PartnerBuilder()
                .WithNumberIssuedPromoCodes(numberIssuedPromoCodes)
                .WithPartnerLimits(new List<PartnerPromoCodeLimit> { limit })
                .Build();

            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            partner.NumberIssuedPromoCodes.Should().Be(numberIssuedPromoCodes);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PreviousLimit_DisablePreviousLimitWhenSettingNewLimit()
        {
            // Arrange
            var partnerId = Guid.NewGuid();

            var activeLimit = new PartnerPromoCodeLimitBuilder().WithEndDate(DateTime.Now.AddDays(1)).Build();

            var partner = new PartnerBuilder()
                .WithPartnerLimits(new List<PartnerPromoCodeLimit> { activeLimit })
                .WithIsActive(true)
                .Build();

            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithEndDate(DateTime.Now.AddDays(1))
                .WithLimit(10)
                .Build();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            partner.PartnerLimits.First().CancelDate.Should().NotBeNull();
            partner.PartnerLimits.Count.Should().Be(2);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_LimitLessThanZero_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.NewGuid();

            var partner = new Fixture().Build<Partner>()
                .OmitAutoProperties()
                .Create();
            
            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithLimit(-1)
                .Build();

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_ShouldUpdatePartner_Successfully()
        {
            // Arrange
            var fixture = new Fixture();
            var partnerId = Guid.NewGuid();
            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            var activeLimit = new PartnerPromoCodeLimitBuilder().WithEndDate(DateTime.Now.AddDays(1)).Build();

            var partner = fixture.Build<Partner>()
                .With(p => p.Id, partnerId)
                .With(p => p.IsActive, true)
                .With(p => p.PartnerLimits, new List<PartnerPromoCodeLimit> { activeLimit })
                .OmitAutoProperties()
                .Create();

            var partnersRepositoryMock = new Mock<IRepository<Partner>>();
            partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            var controller = new PartnersController(partnersRepositoryMock.Object);
            
            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, request);

            // Assert
            partnersRepositoryMock.Verify(repo => repo.UpdateAsync(
                It.Is<Partner>(p => p.Id == partner.Id && p.PartnerLimits.Count > 0)
            ), Times.Once);
        }
    }
}