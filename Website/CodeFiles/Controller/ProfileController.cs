using System;
using System.Web;
using System.Web.Profile;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Web.Controller
{
	/// <summary>
	/// Contains functionality related to managing the user profile.
	/// </summary>
	public static class ProfileController
	{
		#region Public Methods

		/// <overloads>
		/// Gets the gallery-specific user profile for a user.
		/// </overloads>
		/// <summary>
		/// Gets the gallery-specific user profile for the currently logged on user and specified <paramref name="galleryId"/>.
		/// Guaranteed to not return null (returns an empty object if no profile is found).
		/// </summary>
		/// <param name="galleryId">The gallery ID.</param>
		/// <returns>Gets the profile for the current user and the specified gallery.</returns>
		public static IUserGalleryProfile GetProfileForGallery(int galleryId)
		{
			return GetProfileForGallery(Utils.UserName, galleryId);
		}

		/// <summary>
		/// Gets the gallery-specific user profile for the specified <paramref name="userName"/> and <paramref name="galleryId"/>.
		/// Guaranteed to not return null (returns an empty object if no profile is found).
		/// </summary>
		/// <param name="userName">The account name for the user whose profile settings are to be retrieved. You can specify null or an empty string
		/// for anonymous users.</param>
		/// <param name="galleryId">The gallery ID.</param>
		/// <returns>Gets the profile for the specified user and gallery.</returns>
		public static IUserGalleryProfile GetProfileForGallery(string userName, int galleryId)
		{
			return GetProfile(userName).GetGalleryProfile(galleryId);
		}

		/// <overloads>
		/// Gets a user's profile. The UserName property will be an empty string 
		/// for anonymous users and the remaining properties will be set to default values.
		/// </overloads>
		/// <summary>
		/// Gets the profile for the current user.
		/// </summary>
		/// <returns>Gets the profile for the current user.</returns>
		public static IUserProfile GetProfile()
		{
			return GetProfile(Utils.UserName);
		}

		/// <summary>
		/// Gets the user profile for the specified <paramref name="userName" />. Guaranteed to not
		/// return null (returns an empty object if no profile is found).
		/// </summary>
		/// <param name="userName">The account name for the user whose profile settings are to be retrieved. You can specify null or an empty string
		/// for anonymous users.</param>
		/// <returns>Gets the profile for the specified user.</returns>
		public static IUserProfile GetProfile(string userName)
		{
			if (!String.IsNullOrEmpty(userName))
			{
				return GetProfileFromDataStore(userName);
			}
			else
			{
				// Anonymous user. Get from session. If not found in session, return an empty object.
				return GetProfileFromSession(userName) ?? new UserProfile();
			}
		}

		/// <summary>
		/// Saves the specified <paramref name="userProfile" />. Anonymous profiles (those with an empty string in <see cref="IUserProfile.UserName" />)
		/// are saved only to session; profiles for users with accounts are saved to session and also persisted to the data store.
		/// </summary>
		/// <param name="userProfile">The user profile to save.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="userProfile" /> is null.</exception>
		public static void SaveProfile(IUserProfile userProfile)
		{
			if (userProfile == null)
				throw new ArgumentNullException("userProfile");

			if (!String.IsNullOrEmpty(userProfile.UserName))
			{
				SaveProfileToDataStore(userProfile);
			}

			SaveProfileToSession(userProfile);
		}

		/// <summary>
		/// Permanently delete the profile records for the specified <paramref name="userName" />.
		/// </summary>
		/// <param name="userName">The user name that uniquely identifies the user.</param>
		public static void DeleteProfileForUser(string userName)
		{
			Factory.GetDataProvider().Profile_DeleteProfileForUser(userName);
		}

		/// <summary>
		/// Permanently delete the profile records associated with the specified <paramref name="gallery" />.
		/// </summary>
		/// <param name="gallery">The gallery.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="gallery" /> is null.</exception>
		public static void DeleteProfileForGallery(Business.Gallery gallery)
		{
			if (gallery == null)
				throw new ArgumentNullException("gallery");

			Factory.GetDataProvider().Profile_DeleteProfilesForGallery(gallery.GalleryId);
		}

		/// <summary>
		/// Removes the current user's profile from session. This session key has a unique name based on the session ID and logged-on 
		/// user's name. This function is not critical for security or correctness, but is useful in keeping the session cleared of unused items. When
		/// a user logs on or off, their username changes - and therefore the name of the session key changes, which causes the next call to 
		/// retrieve the user's profile to return nothing from the cache, which forces a retrieval from the database. Thus the correct profile will
		/// always be retrieved, even if this function is not invoked during a logon/logoff event.
		/// </summary>
		public static void RemoveProfileFromSession()
		{
			if (HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session.Remove(GetSessionKeyNameForProfile(Utils.UserName));
			}
		}

		#endregion

		#region Private Functions

		private static IUserProfile GetProfileFromDataStore(string userName)
		{
			// We can save a hit to the database by retrieving the profile from session once it has been initially retrieved from the data store.
			IUserProfile profile = GetProfileFromSession(userName);
			if (profile != null)
			{
				// We found a profile, so no need to go to the data store.
				return profile;
			}

			// No profile in session. Get from data store and then store in session.
			profile = Factory.LoadUserProfile(userName);

			// Save to session for quicker access next time.
			SaveProfileToSession(profile);

			return profile;
		}

		/// <summary>
		/// Gets the user's profile from session. Returns null if no object is found.
		/// </summary>
		/// <param name="userName">Name of the user. May be null.</param>
		/// <returns>Returns </returns>
		private static IUserProfile GetProfileFromSession(string userName)
		{
			IUserProfile pc = null;

			if (HttpContext.Current.Session != null)
			{
				pc = HttpContext.Current.Session[GetSessionKeyNameForProfile(userName)] as IUserProfile;
			}

			return pc;
		}

		private static void SaveProfileToDataStore(IUserProfile userProfile)
		{
			Factory.GetDataProvider().Profile_Save(userProfile);
		}

		private static void SaveProfileToSession(IUserProfile userProfile)
		{
			if (HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session[GetSessionKeyNameForProfile(userProfile.UserName)] = userProfile;
			}
		}


		/// <summary>
		/// Gets the name that identifies the session item that holds the current user's profile. The key is unique for each session ID / username 
		/// combination. This prevents us from retrieving the wrong profile after logon/logout events.
		/// </summary>
		/// <param name="userName">Name of the user. May be null.</param>
		/// <returns>Returns the name that identifies the session item that holds the current user's profile.</returns>
		private static string GetSessionKeyNameForProfile(string userName)
		{
			return String.Concat(HttpContext.Current.Session.SessionID, "_", userName, "_Profile");
		}

		#endregion
	}
}
