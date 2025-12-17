// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Ecommerce.EnterpriseServer
{
	public enum AIMField
	{
		ResponseCode = 0,
		ResponseSubCode = 1,
		ResponseReasonCode = 2,
		ResponseReasonText = 3,
		ApprovalCode = 4,
		AvsResultCode = 5,
		TransactionId = 6,
		RespInvoiceNum = 7,
		Description = 8,
		RespAmount = 9,
		RespPaymentMethod = 10,
		TransactionType = 11,
		RespCustomerId = 12,
		RespFirstName = 13,
		RespLastName = 14,
		RespCompany = 15,
		BillingAddress = 16,
		RespCity = 17,
		RespState = 18,
		Zip = 19,
		Country = 20,
		Phone = 21,
		Fax = 22,
		Email = 23,
		ShipToFirstName = 24,
		ShipToLastName = 25,
		ShipToCompany = 26,
		ShipToAddress = 27,
		ShipToCity = 28,
		ShipToState = 29,
		ShipToZip = 30,
		ShipToCountry = 31,
		TaxAmount = 32,
		DutyAmount = 33,
		FreightAmount = 34,
		TaxExemptFlag = 35,
		PurchaseOrderNumber = 36,
		ResponseSignature = 37,
		CardVerificationCode = 38
	};

	public class AIMResponse
	{
		private string[] _aimData;
		private string _rawResponse;
		private int _aimLength;
		private char _delimChar = '|';

		public string RawResponse
		{
			get { return _rawResponse; }
		}

		public AIMResponse(Stream stream, char delimiter)
		{
			_delimChar = delimiter;
			StreamReader sr = new StreamReader(stream);
			string response = sr.ReadToEnd();
			sr.Close();

			Initialize(response);
		}

		public AIMResponse(string response)
		{
			Initialize(response);
		}

		private void Initialize(string response)
		{
			if (string.IsNullOrEmpty(response))
				throw new Exception("Response data is empty.");

			_aimData = response.Split(_delimChar);
			_aimLength = _aimData.Length;

			if (_aimData.Length == 0 && response.Length > 0)
				throw new Exception("Invalid response data format.");

			_rawResponse = response;
		}

		public string this[AIMField field]
		{
			get
			{
				int index = (int)field;

				if (index < _aimLength)
					return _aimData[index];

				return null;
			}
		}
	}

}
