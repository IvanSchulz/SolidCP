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
using System.Collections.Generic;
using System.Text;

namespace FuseCP.Ecommerce.EnterpriseServer
{
	public class OfflinePayment : SystemPluginBase, IPaymentGatewayProvider
	{
        public const string DEFAULT_TRANSACTION_FORMAT = "yyyyMMdd-[INVOICE_ID]";
		public const string ErrorPrefix = "OfflinePayment.";
		/// <summary>
		/// Gets payment prefix
		/// </summary>
        public string TransactionNumberFormat
        {
            get
            {
                string formatStr = PluginSettings[OffPaymentSettings.TRANSACTION_NUMBER_FORMAT];
                //
                if (String.IsNullOrEmpty(formatStr))
                    formatStr = DEFAULT_TRANSACTION_FORMAT;
                //
                return formatStr;
            }
        }

		public bool AutoApprove
		{
			get { return Convert.ToBoolean(PluginSettings[OffPaymentSettings.AUTO_APPROVE]); }
		}

		public OfflinePayment()
		{
		}

		#region IPaymentGatewayProvider Members

		public TransactionResult SubmitPaymentTransaction(CheckoutDetails details)
		{
			TransactionResult result = new TransactionResult();

            // 1. Process date and time variables
            string transactionNumber = DateTime.Now.ToString(TransactionNumberFormat);
            // 2. Process E-Commerce variables
            transactionNumber = transactionNumber.Replace("[INVOICE_ID]", details[CheckoutKeys.InvoiceNumber]);

			// transaction is ok
			result.Succeed = true;
			// set transation id
			result.TransactionId = transactionNumber;
			// status code is empty
			result.StatusCode = String.Empty;
			// no response available
			result.RawResponse = "No response available";

			// check payment approval setting
			if (AutoApprove)
				result.TransactionStatus = TransactionStatus.Approved;
			else
				result.TransactionStatus = TransactionStatus.Pending;

			// return result
			return result;
		}

		#endregion
	}
}
