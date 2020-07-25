﻿using System;
using System.Collections.Generic;
using System.Text;
using Asn1;

namespace PcapAnalyzer
{
    public static class KerberosPacketParser
    {
        

        // 13 - Get service ticket (Response to KRB_TGS_REQ request)
        public enum MessageType : byte
        {
            krb_tgs_rep = 13,
        }

        public static object GetKerberosPacket(byte[] kerberosBuffer)
        {
            object result = null;
            byte[] asn_buffer = AsnIO.FindBER(kerberosBuffer);

            if (asn_buffer is null)
            {
                throw new Exception("Not a valid Kerberos V5 data");
            }

            AsnElt asn_object = AsnElt.Decode(asn_buffer);

            // Get the application number
            switch (asn_object.TagValue)
            {
                case (int)MessageType.krb_tgs_rep:
                    result = new KerberosTgsRepPacket(kdc_rep: asn_object.Sub[0].Sub);
                    break;
            }

            return result;
        }

    }
}
