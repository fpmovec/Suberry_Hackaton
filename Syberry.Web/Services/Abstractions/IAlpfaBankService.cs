using Syberry.Web.Models;
using Syberry.Web.Models.Dto;

namespace Syberry.Web.Services.Abstractions;

public interface IAlpfaBankService
{
    Task<Bank> AlpfaBankRates();
}