using Microsoft.Win32;
using System;
using IESandDACadmt.Model.Logging;

namespace IESandDACadmt.Model
{
    public static class RegistryReader
    {
        public static ActionOutcome ReadRegistryLocalMachineString(string registryLocation, string registryItem)
        {
            ActionOutcome result = new ActionOutcome();

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryLocation))
                {
                    if (key != null)
                    {
                        String strKey = (string)key.GetValue(registryItem);
                        if (strKey != null)
                        {
                            result.Success = true;
                            result.Message = strKey;
                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Registry Item " + registryItem + " not found.";
                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Registry Key " + registryLocation + " not found.";
                        return result;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                result.Success = false;
                result.Message = "Registry Key " + registryLocation + " is closed.";
                return result;
            }
            catch (System.Security.SecurityException)
            {
                result.Success = false;
                result.Message = "Insufficient rights to access registry location " + registryLocation;
                return result;
            }
            
        }
    }
}
