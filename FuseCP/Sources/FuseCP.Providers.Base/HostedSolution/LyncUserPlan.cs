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

namespace FuseCP.Providers.HostedSolution
{
    public class LyncUserPlan
    {
        int lyncUserPlanId;
        int itemId;
        string lyncUserPlanName;

        bool im;
        bool federation;
        bool conferencing;
        bool enterpriseVoice;
        bool mobility;
        bool mobilityEnableOutsideVoice;
        LyncVoicePolicyType voicePolicy;
        int lyncUserPlanType;

        bool isDefault;

        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }

        public int LyncUserPlanId
        {
            get { return this.lyncUserPlanId; }
            set { this.lyncUserPlanId = value; }
        }

        public string LyncUserPlanName
        {
            get { return this.lyncUserPlanName; }
            set { this.lyncUserPlanName = value; }
        }

        public int LyncUserPlanType
        {
            get { return this.lyncUserPlanType; }
            set { this.lyncUserPlanType = value; }
        }


        public bool IM
        {
            get { return this.im; }
            set { this.im = value; }
        }

        public bool IsDefault
        {
            get { return this.isDefault; }
            set { this.isDefault = value; }
        }

        public bool Federation
        {
            get { return this.federation; }
            set { this.federation = value; }
        }

        public bool Conferencing
        {
            get { return this.conferencing; }
            set { this.conferencing = value; }
        }

        public bool EnterpriseVoice
        {
            get { return this.enterpriseVoice; }
            set { this.enterpriseVoice = value; }
        }

        public bool Mobility
        {
            get { return this.mobility; }
            set { this.mobility = value; }
        }

        public bool MobilityEnableOutsideVoice
        {
            get { return this.mobilityEnableOutsideVoice; }
            set { this.mobilityEnableOutsideVoice = value; }
        }

        public LyncVoicePolicyType VoicePolicy
        {
            get { return this.voicePolicy; }
            set { this.voicePolicy = value; }
        }

        bool remoteUserAccess;
        bool publicIMConnectivity;

        bool allowOrganizeMeetingsWithExternalAnonymous;

        int telephony;

        string serverURI;

        string archivePolicy;
        string telephonyDialPlanPolicy;
        string telephonyVoicePolicy;

        public bool RemoteUserAccess
        {
            get { return this.remoteUserAccess; }
            set { this.remoteUserAccess = value; }
        }
        public bool PublicIMConnectivity
        {
            get { return this.publicIMConnectivity; }
            set { this.publicIMConnectivity = value; }
        }

        public bool AllowOrganizeMeetingsWithExternalAnonymous
        {
            get { return this.allowOrganizeMeetingsWithExternalAnonymous; }
            set { this.allowOrganizeMeetingsWithExternalAnonymous = value; }
        }

        public int Telephony
        {
            get { return this.telephony; }
            set { this.telephony = value; }
        }

        public string ServerURI
        {
            get { return this.serverURI; }
            set { this.serverURI = value; }
        }

        public string ArchivePolicy
        {
            get { return this.archivePolicy; }
            set { this.archivePolicy = value; }
        }

        public string TelephonyDialPlanPolicy
        {
            get { return this.telephonyDialPlanPolicy; }
            set { this.telephonyDialPlanPolicy = value; }
        }
        public string TelephonyVoicePolicy
        {
            get { return this.telephonyVoicePolicy; }
            set { this.telephonyVoicePolicy = value; }
        }
    }
}
