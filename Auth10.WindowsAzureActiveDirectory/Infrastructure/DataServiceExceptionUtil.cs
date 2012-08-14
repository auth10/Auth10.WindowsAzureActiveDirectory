/// <copyright file="DataServiceUtil.cs" company="Microsoft">
/// Copyright (c) Microsoft Corporation. All rights reserved.
/// </copyright>
/// <Summary> 
/// Project:      AzureAD Sample Application
/// Copyright (c) Microsoft Corporation.
/// 
/// This source is subject to the Sample Client End User License Agreement included in this project
/// Sample Client EULA.rtf
/// 
/// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
/// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
/// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
/// </summary>
namespace Auth10.WindowsAzureActiveDirectory.Infrastructure
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// Helper class to de-serialize DataServiceExceptions thrown by an ADO.NET Data Service
    /// </summary>
    public static class DataServiceExceptionUtil
    {
        /// <summary>
        /// Xml serializer.
        /// </summary>
        private static DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(ErrorResponseEx));

        /// <summary>
        /// Serialize the error message to JSON / Xml
        /// </summary>
        /// <param name="ex">Exception to be deseralized</param>
        /// <returns>Deserialized error message.</returns>
        public static ErrorResponseEx DeserializeErrorMessage(Exception ex)
        {
            ErrorResponseEx errorResponse = null;
            string errorMessage = ex.InnerException.Message;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(errorMessage)))
                {
                    errorResponse = (ErrorResponseEx)xmlSerializer.ReadObject(ms);
                }   
            }
            
            return errorResponse;
        }        
    }
}