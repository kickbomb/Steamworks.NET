// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2016 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

using System;
using System.Runtime.InteropServices;

namespace Steamworks {
	public static class SteamInventory {
		/// <summary>
		/// <para> INVENTORY ASYNC RESULT MANAGEMENT</para>
		/// <para> Asynchronous inventory queries always output a result handle which can be used with</para>
		/// <para> GetResultStatus, GetResultItems, etc. A SteamInventoryResultReady_t callback will</para>
		/// <para> be triggered when the asynchronous result becomes ready (or fails).</para>
		/// <para> Find out the status of an asynchronous inventory result handle. Possible values:</para>
		/// <para>  k_EResultPending - still in progress</para>
		/// <para>  k_EResultOK - done, result ready</para>
		/// <para>  k_EResultExpired - done, result ready, maybe out of date (see DeserializeResult)</para>
		/// <para>  k_EResultInvalidParam - ERROR: invalid API call parameters</para>
		/// <para>  k_EResultServiceUnavailable - ERROR: service temporarily down, you may retry later</para>
		/// <para>  k_EResultLimitExceeded - ERROR: operation would exceed per-user inventory limits</para>
		/// <para>  k_EResultFail - ERROR: unknown / generic error</para>
		/// </summary>
		public static EResult GetResultStatus(SteamInventoryResult_t resultHandle) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetResultStatus(resultHandle);
		}

		/// <summary>
		/// <para> Copies the contents of a result set into a flat array. The specific</para>
		/// <para> contents of the result set depend on which query which was used.</para>
		/// </summary>
		public static bool GetResultItems(SteamInventoryResult_t resultHandle, SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetResultItems(resultHandle, pOutItemsArray, ref punOutItemsArraySize);
		}

		/// <summary>
		/// <para> Returns the server time at which the result was generated. Compare against</para>
		/// <para> the value of IClientUtils::GetServerRealTime() to determine age.</para>
		/// </summary>
		public static uint GetResultTimestamp(SteamInventoryResult_t resultHandle) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetResultTimestamp(resultHandle);
		}

		/// <summary>
		/// <para> Returns true if the result belongs to the target steam ID, false if the</para>
		/// <para> result does not. This is important when using DeserializeResult, to verify</para>
		/// <para> that a remote player is not pretending to have a different user's inventory.</para>
		/// </summary>
		public static bool CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_CheckResultSteamID(resultHandle, steamIDExpected);
		}

		/// <summary>
		/// <para> Destroys a result handle and frees all associated memory.</para>
		/// </summary>
		public static void DestroyResult(SteamInventoryResult_t resultHandle) {
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInventory_DestroyResult(resultHandle);
		}

		/// <summary>
		/// <para> INVENTORY ASYNC QUERY</para>
		/// <para> Captures the entire state of the current user's Steam inventory.</para>
		/// <para> You must call DestroyResult on this handle when you are done with it.</para>
		/// <para> Returns false and sets *pResultHandle to zero if inventory is unavailable.</para>
		/// <para> Note: calls to this function are subject to rate limits and may return</para>
		/// <para> cached results if called too frequently. It is suggested that you call</para>
		/// <para> this function only when you are about to display the user's full inventory,</para>
		/// <para> or if you expect that the inventory may have changed.</para>
		/// </summary>
		public static bool GetAllItems(out SteamInventoryResult_t pResultHandle) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetAllItems(out pResultHandle);
		}

		/// <summary>
		/// <para> Captures the state of a subset of the current user's Steam inventory,</para>
		/// <para> identified by an array of item instance IDs. The results from this call</para>
		/// <para> can be serialized and passed to other players to "prove" that the current</para>
		/// <para> user owns specific items, without exposing the user's entire inventory.</para>
		/// <para> For example, you could call GetItemsByID with the IDs of the user's</para>
		/// <para> currently equipped cosmetic items and serialize this to a buffer, and</para>
		/// <para> then transmit this buffer to other players upon joining a game.</para>
		/// </summary>
		public static bool GetItemsByID(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetItemsByID(out pResultHandle, pInstanceIDs, unCountInstanceIDs);
		}

		/// <summary>
		/// <para> RESULT SERIALIZATION AND AUTHENTICATION</para>
		/// <para> Serialized result sets contain a short signature which can't be forged</para>
		/// <para> or replayed across different game sessions. A result set can be serialized</para>
		/// <para> on the local client, transmitted to other players via your game networking,</para>
		/// <para> and deserialized by the remote players. This is a secure way of preventing</para>
		/// <para> hackers from lying about posessing rare/high-value items.</para>
		/// <para> Serializes a result set with signature bytes to an output buffer. Pass</para>
		/// <para> NULL as an output buffer to get the required size via punOutBufferSize.</para>
		/// <para> The size of a serialized result depends on the number items which are being</para>
		/// <para> serialized. When securely transmitting items to other players, it is</para>
		/// <para> recommended to use "GetItemsByID" first to create a minimal result set.</para>
		/// <para> Results have a built-in timestamp which will be considered "expired" after</para>
		/// <para> an hour has elapsed. See DeserializeResult for expiration handling.</para>
		/// </summary>
		public static bool SerializeResult(SteamInventoryResult_t resultHandle, byte[] pOutBuffer, out uint punOutBufferSize) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_SerializeResult(resultHandle, pOutBuffer, out punOutBufferSize);
		}

		/// <summary>
		/// <para> Deserializes a result set and verifies the signature bytes. Returns false</para>
		/// <para> if bRequireFullOnlineVerify is set but Steam is running in Offline mode.</para>
		/// <para> Otherwise returns true and then delivers error codes via GetResultStatus.</para>
		/// <para> The bRESERVED_MUST_BE_FALSE flag is reserved for future use and should not</para>
		/// <para> be set to true by your game at this time.</para>
		/// <para> DeserializeResult has a potential soft-failure mode where the handle status</para>
		/// <para> is set to k_EResultExpired. GetResultItems() still succeeds in this mode.</para>
		/// <para> The "expired" result could indicate that the data may be out of date - not</para>
		/// <para> just due to timed expiration (one hour), but also because one of the items</para>
		/// <para> in the result set may have been traded or consumed since the result set was</para>
		/// <para> generated. You could compare the timestamp from GetResultTimestamp() to</para>
		/// <para> ISteamUtils::GetServerRealTime() to determine how old the data is. You could</para>
		/// <para> simply ignore the "expired" result code and continue as normal, or you</para>
		/// <para> could challenge the player with expired data to send an updated result set.</para>
		/// </summary>
		public static bool DeserializeResult(out SteamInventoryResult_t pOutResultHandle, byte[] pBuffer, uint unBufferSize, bool bRESERVED_MUST_BE_FALSE = false) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_DeserializeResult(out pOutResultHandle, pBuffer, unBufferSize, bRESERVED_MUST_BE_FALSE);
		}

		/// <summary>
		/// <para> INVENTORY ASYNC MODIFICATION</para>
		/// <para> GenerateItems() creates one or more items and then generates a SteamInventoryCallback_t</para>
		/// <para> notification with a matching nCallbackContext parameter. This API is insecure, and could</para>
		/// <para> be abused by hacked clients. It is, however, very useful as a development cheat or as</para>
		/// <para> a means of prototyping item-related features for your game. The use of GenerateItems can</para>
		/// <para> be restricted to certain item definitions or fully blocked via the Steamworks website.</para>
		/// <para> If punArrayQuantity is not NULL, it should be the same length as pArrayItems and should</para>
		/// <para> describe the quantity of each item to generate.</para>
		/// </summary>
		public static bool GenerateItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint[] punArrayQuantity, uint unArrayLength) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GenerateItems(out pResultHandle, pArrayItemDefs, punArrayQuantity, unArrayLength);
		}

		/// <summary>
		/// <para> GrantPromoItems() checks the list of promotional items for which the user may be eligible</para>
		/// <para> and grants the items (one time only).  On success, the result set will include items which</para>
		/// <para> were granted, if any. If no items were granted because the user isn't eligible for any</para>
		/// <para> promotions, this is still considered a success.</para>
		/// </summary>
		public static bool GrantPromoItems(out SteamInventoryResult_t pResultHandle) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GrantPromoItems(out pResultHandle);
		}

		/// <summary>
		/// <para> AddPromoItem() / AddPromoItems() are restricted versions of GrantPromoItems(). Instead of</para>
		/// <para> scanning for all eligible promotional items, the check is restricted to a single item</para>
		/// <para> definition or set of item definitions. This can be useful if your game has custom UI for</para>
		/// <para> showing a specific promo item to the user.</para>
		/// </summary>
		public static bool AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_AddPromoItem(out pResultHandle, itemDef);
		}

		public static bool AddPromoItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayItemDefs, uint unArrayLength) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_AddPromoItems(out pResultHandle, pArrayItemDefs, unArrayLength);
		}

		/// <summary>
		/// <para> ConsumeItem() removes items from the inventory, permanently. They cannot be recovered.</para>
		/// <para> Not for the faint of heart - if your game implements item removal at all, a high-friction</para>
		/// <para> UI confirmation process is highly recommended. Similar to GenerateItems, punArrayQuantity</para>
		/// <para> can be NULL or else an array of the same length as pArrayItems which describe the quantity</para>
		/// <para> of each item to destroy. ConsumeItem can be restricted to certain item definitions or</para>
		/// <para> fully blocked via the Steamworks website to minimize support/abuse issues such as the</para>
		/// <para> clasic "my brother borrowed my laptop and deleted all of my rare items".</para>
		/// </summary>
		public static bool ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_ConsumeItem(out pResultHandle, itemConsume, unQuantity);
		}

		/// <summary>
		/// <para> ExchangeItems() is an atomic combination of GenerateItems and DestroyItems. It can be</para>
		/// <para> used to implement crafting recipes or transmutations, or items which unpack themselves</para>
		/// <para> into other items. Like GenerateItems, this is a flexible and dangerous API which is</para>
		/// <para> meant for rapid prototyping. You can configure restrictions on ExchangeItems via the</para>
		/// <para> Steamworks website, such as limiting it to a whitelist of input/output combinations</para>
		/// <para> corresponding to recipes.</para>
		/// <para> (Note: although GenerateItems may be hard or impossible to use securely in your game,</para>
		/// <para> ExchangeItems is perfectly reasonable to use once the whitelists are set accordingly.)</para>
		/// </summary>
		public static bool ExchangeItems(out SteamInventoryResult_t pResultHandle, SteamItemDef_t[] pArrayGenerate, uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, SteamItemInstanceID_t[] pArrayDestroy, uint[] punArrayDestroyQuantity, uint unArrayDestroyLength) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_ExchangeItems(out pResultHandle, pArrayGenerate, punArrayGenerateQuantity, unArrayGenerateLength, pArrayDestroy, punArrayDestroyQuantity, unArrayDestroyLength);
		}

		/// <summary>
		/// <para> TransferItemQuantity() is intended for use with items which are "stackable" (can have</para>
		/// <para> quantity greater than one). It can be used to split a stack into two, or to transfer</para>
		/// <para> quantity from one stack into another stack of identical items. To split one stack into</para>
		/// <para> two, pass k_SteamItemInstanceIDInvalid for itemIdDest and a new item will be generated.</para>
		/// </summary>
		public static bool TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_TransferItemQuantity(out pResultHandle, itemIdSource, unQuantity, itemIdDest);
		}

		/// <summary>
		/// <para> TIMED DROPS AND PLAYTIME CREDIT</para>
		/// <para> Applications which use timed-drop mechanics should call SendItemDropHeartbeat() when</para>
		/// <para> active gameplay begins, and at least once every two minutes afterwards. The backend</para>
		/// <para> performs its own time calculations, so the precise timing of the heartbeat is not</para>
		/// <para> critical as long as you send at least one heartbeat every two minutes. Calling the</para>
		/// <para> function more often than that is not harmful, it will simply have no effect. Note:</para>
		/// <para> players may be able to spoof this message by hacking their client, so you should not</para>
		/// <para> attempt to use this as a mechanism to restrict playtime credits. It is simply meant</para>
		/// <para> to distinguish between being in any kind of gameplay situation vs the main menu or</para>
		/// <para> a pre-game launcher window. (If you are stingy with handing out playtime credit, it</para>
		/// <para> will only encourage players to run bots or use mouse/kb event simulators.)</para>
		/// <para> Playtime credit accumulation can be capped on a daily or weekly basis through your</para>
		/// <para> Steamworks configuration.</para>
		/// </summary>
		public static void SendItemDropHeartbeat() {
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamInventory_SendItemDropHeartbeat();
		}

		/// <summary>
		/// <para> Playtime credit must be consumed and turned into item drops by your game. Only item</para>
		/// <para> definitions which are marked as "playtime item generators" can be spawned. The call</para>
		/// <para> will return an empty result set if there is not enough playtime credit for a drop.</para>
		/// <para> Your game should call TriggerItemDrop at an appropriate time for the user to receive</para>
		/// <para> new items, such as between rounds or while the player is dead. Note that players who</para>
		/// <para> hack their clients could modify the value of "dropListDefinition", so do not use it</para>
		/// <para> to directly control rarity. It is primarily useful during testing and development,</para>
		/// <para> where you may wish to perform experiments with different types of drops.</para>
		/// </summary>
		public static bool TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_TriggerItemDrop(out pResultHandle, dropListDefinition);
		}

		/// <summary>
		/// <para> IN-GAME TRADING</para>
		/// <para> TradeItems() implements limited in-game trading of items, if you prefer not to use</para>
		/// <para> the overlay or an in-game web browser to perform Steam Trading through the website.</para>
		/// <para> You should implement a UI where both players can see and agree to a trade, and then</para>
		/// <para> each client should call TradeItems simultaneously (+/- 5 seconds) with matching</para>
		/// <para> (but reversed) parameters. The result is the same as if both players performed a</para>
		/// <para> Steam Trading transaction through the web. Each player will get an inventory result</para>
		/// <para> confirming the removal or quantity changes of the items given away, and the new</para>
		/// <para> item instance id numbers and quantities of the received items.</para>
		/// <para> (Note: new item instance IDs are generated whenever an item changes ownership.)</para>
		/// </summary>
		public static bool TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, SteamItemInstanceID_t[] pArrayGive, uint[] pArrayGiveQuantity, uint nArrayGiveLength, SteamItemInstanceID_t[] pArrayGet, uint[] pArrayGetQuantity, uint nArrayGetLength) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_TradeItems(out pResultHandle, steamIDTradePartner, pArrayGive, pArrayGiveQuantity, nArrayGiveLength, pArrayGet, pArrayGetQuantity, nArrayGetLength);
		}

		/// <summary>
		/// <para> ITEM DEFINITIONS</para>
		/// <para> Item definitions are a mapping of "definition IDs" (integers between 1 and 1000000)</para>
		/// <para> to a set of string properties. Some of these properties are required to display items</para>
		/// <para> on the Steam community web site. Other properties can be defined by applications.</para>
		/// <para> Use of these functions is optional; there is no reason to call LoadItemDefinitions</para>
		/// <para> if your game hardcodes the numeric definition IDs (eg, purple face mask = 20, blue</para>
		/// <para> weapon mod = 55) and does not allow for adding new item types without a client patch.</para>
		/// <para> LoadItemDefinitions triggers the automatic load and refresh of item definitions.</para>
		/// <para> Every time new item definitions are available (eg, from the dynamic addition of new</para>
		/// <para> item types while players are still in-game), a SteamInventoryDefinitionUpdate_t</para>
		/// <para> callback will be fired.</para>
		/// </summary>
		public static bool LoadItemDefinitions() {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_LoadItemDefinitions();
		}

		/// <summary>
		/// <para> GetItemDefinitionIDs returns the set of all defined item definition IDs (which are</para>
		/// <para> defined via Steamworks configuration, and not necessarily contiguous integers).</para>
		/// <para> If pItemDefIDs is null, the call will return true and *punItemDefIDsArraySize will</para>
		/// <para> contain the total size necessary for a subsequent call. Otherwise, the call will</para>
		/// <para> return false if and only if there is not enough space in the output array.</para>
		/// </summary>
		public static bool GetItemDefinitionIDs(SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize) {
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamInventory_GetItemDefinitionIDs(pItemDefIDs, out punItemDefIDsArraySize);
		}

		/// <summary>
		/// <para> GetItemDefinitionProperty returns a string property from a given item definition.</para>
		/// <para> Note that some properties (for example, "name") may be localized and will depend</para>
		/// <para> on the current Steam language settings (see ISteamApps::GetCurrentGameLanguage).</para>
		/// <para> Property names are always composed of ASCII letters, numbers, and/or underscores.</para>
		/// <para> Pass a NULL pointer for pchPropertyName to get a comma - separated list of available</para>
		/// <para> property names.</para>
		/// </summary>
		public static bool GetItemDefinitionProperty(SteamItemDef_t iDefinition, string pchPropertyName, out string pchValueBuffer, ref uint punValueBufferSize) {
			InteropHelp.TestIfAvailableClient();
			IntPtr pchValueBuffer2 = Marshal.AllocHGlobal((int)punValueBufferSize);
			using (var pchPropertyName2 = new InteropHelp.UTF8StringHandle(pchPropertyName)) {
				bool ret = NativeMethods.ISteamInventory_GetItemDefinitionProperty(iDefinition, pchPropertyName2, pchValueBuffer2, ref punValueBufferSize);
				pchValueBuffer = ret ? InteropHelp.PtrToStringUTF8(pchValueBuffer2) : null;
				Marshal.FreeHGlobal(pchValueBuffer2);
				return ret;
			}
		}
	}
}