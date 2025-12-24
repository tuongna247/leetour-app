using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vinaday.Core.Repositories;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public class LottoService : Service<Lotto>, ILottoService, IService<Lotto>
	{
		private readonly IRepositoryAsync<Lotto> _lottoRepository;

		private readonly IRepositoryAsync<LottoDetail> _lottoDetailRepository;

		public LottoService(IRepositoryAsync<Lotto> lottoRepository, IRepositoryAsync<LottoDetail> lottoDetailRepository) : base(lottoRepository)
		{
			this._lottoRepository = lottoRepository;
			this._lottoDetailRepository = lottoDetailRepository;
		}

		public LottoDetail AddLottoDetail(LottoDetail lottoDetail)
		{
			this._lottoDetailRepository.Insert(lottoDetail);
			return lottoDetail;
		}

		public bool CheckCode(string code)
		{
			return this._lottoDetailRepository.CheckCode(code);
		}

		public bool CheckLotto(int lottoId, string phone, string lastId)
		{
			IEnumerable<LottoDetail> lottoDetailsByLottoId = this._lottoDetailRepository.GetLottoDetailsByLottoId(lottoId);
			bool flag = lottoDetailsByLottoId.FirstOrDefault<LottoDetail>((LottoDetail l) => (!l.Status ? false : (l.LastIdNumber == lastId ? true : l.Phone == phone))) != null;
			return flag;
		}

		public Lotto GetLotto(int id)
		{
			return this._lottoRepository.GetLotto(id);
		}

		public Lotto GetLottoByName(string name)
		{
			return this._lottoRepository.GetLottoByName(name);
		}

		public List<LottoDetail> GetLottoDetails(int id)
		{
			List<LottoDetail> list = this._lottoDetailRepository.GetLottoDetailsByLottoId(id).ToList<LottoDetail>();
			return list;
		}

		public List<Lotto> GetLottos()
		{
			return this._lottoRepository.GetLottos().ToList<Lotto>();
		}
	}
}