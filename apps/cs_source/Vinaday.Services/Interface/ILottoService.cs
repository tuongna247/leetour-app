using Service.Pattern;
using System;
using System.Collections.Generic;
using Vinaday.Data.Models;

namespace Vinaday.Services
{
	public interface ILottoService : IService<Lotto>
	{
		LottoDetail AddLottoDetail(LottoDetail lottoDetail);

		bool CheckCode(string code);

		bool CheckLotto(int lottoId, string phone, string lastId);

		Lotto GetLotto(int id);

		Lotto GetLottoByName(string name);

		List<LottoDetail> GetLottoDetails(int id);

		List<Lotto> GetLottos();
	}
}