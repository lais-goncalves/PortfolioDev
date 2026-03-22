using AutoMapper;
using PortfolioDev.Application.DTOs;
using PortfolioDev.Application.DTOs.Atualizacao;
using PortfolioDev.Application.DTOs.Identity;
using PortfolioDev.Application.DTOs.Registro;
using PortfolioDev.Application.DTOs.Registro.Identity;
using PortfolioDev.Domain.Models;
using PortfolioDev.Domain.Models.Identity;

namespace PortfolioDev.Application.Helpers.Mapper;

public class PlataformaDevsProfile : Profile
{
	public PlataformaDevsProfile()
	{
		CreateMap<Usuario, UsuarioDto>().ReverseMap();
		CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();
		CreateMap<Usuario, UsuarioRegistroDto>().ReverseMap();

		CreateMap<Portfolio, PortfolioDto>().ReverseMap();
		CreateMap<Portfolio, PortfolioRegistroDto>().ReverseMap();
		CreateMap<Portfolio, PortfolioAtualizacaoDto>().ReverseMap();

		CreateMap<Projeto, ProjetoDto>().ReverseMap();
		CreateMap<Projeto, ProjetoRegistroDto>().ReverseMap();
		CreateMap<Projeto, ProjetoAtualizacaoDto>().ReverseMap();
	}
}