/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/dem1se/SDVMods
**
*************************************************/

using Dem1se.RecurringReminders.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using System;

namespace Dem1se.RecurringReminders.Mpm
{
    public interface IMobilePhoneApi
    {
        bool AddApp(string id, string name, Action action, Texture2D icon);
        Vector2 GetScreenPosition();
        Vector2 GetScreenSize();
        Vector2 GetScreenSize(bool rotated);
        Rectangle GetPhoneRectangle();
        Rectangle GetScreenRectangle();
        bool GetPhoneRotated();
        void SetPhoneRotated(bool value);
        bool GetPhoneOpened();
        void SetPhoneOpened(bool value);
        bool GetAppRunning();
        void SetAppRunning(bool value);
        string GetRunningApp();
        void SetRunningApp(string value);
    }

    public static class MobilePhoneMod
    {
        public static void HookToMobilePhoneMod(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            IMobilePhoneApi api = Data.Helper.ModRegistry.GetApi<IMobilePhoneApi>("aedenthorn.MobilePhone");
            if (api != null)
            {
                Texture2D appIcon = Data.Helper.Content.Load<Texture2D>(System.IO.Path.Combine("assets", "mpmIcon.png"));
                bool success = api.AddApp(Data.Helper.ModRegistry.ModID, "Recurring Reminders", ModEntry.ShowReminderMenu, appIcon);
                Data.Monitor.Log($"Loaded phone app successfully: {success}", LogLevel.Debug);
            }
        }
    }

}
