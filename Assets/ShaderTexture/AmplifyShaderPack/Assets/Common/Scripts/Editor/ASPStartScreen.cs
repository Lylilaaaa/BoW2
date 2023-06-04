// Amplify Shader Pack
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AmplifyShaderPack
{
	public class ASPStartScreen : EditorWindow
	{
		[MenuItem( "Window/Amplify Shader Pack/Start Screen", false, 1998 )]
		public static void Init()
		{
			ASPStartScreen window = (ASPStartScreen)GetWindow( typeof( ASPStartScreen ) , true, "Amplify Shader Pack Start Screen" );
			window.minSize = new Vector2( 660, 700 );
			window.maxSize = new Vector2( 660, 700 );
			window.Show();
		}

		private static readonly string HDRPSamples7to9GUID = "abf8c3e18e883894aa7a5cf7dc6a5408";
		private static readonly string HDRPSamples10to11GUID = "312b55e12eefb9b4aba6ebeb4ad6f35a";
		private static readonly string URPSamples10to11GUID = "47222e91b38f7f943a2c717aca57ed58";
		private static readonly string URPSamples7to9GUID = "a4ba258cacd903245bb6a04777c45689";
		private static readonly string BiRPSamplesGUID = "cc34b441a892177478d7932a061167f7";



		private static readonly string ChangeLogGUID = "c8b8d880514bcf14aa8b2bbb7d362e36";
	

		private static readonly string ASEIconGUID = "24f41dcf57cec2745a368c3504053d60";
		private static readonly string ASPIconGUID = "bc13bd608b59d9d4aa6c5efd2294b54e";

		public static readonly string ChangelogURL = "http://amplify.pt/Banner/ASPchangelog.json";

		private static readonly string HDRPCaralogueURL = "http://wiki.amplify.pt/index.php?title=Unity_Products:Amplify_Shader_Pack/HDRP";
		private static readonly string URPCatalogueURL = "http://wiki.amplify.pt/index.php?title=Unity_Products:Amplify_Shader_Pack/URP";
		private static readonly string BuiltinCatalogueURL = "http://wiki.amplify.pt/index.php?title=Unity_Products:Amplify_Shader_Pack/BuiltIn";


		private static readonly string DiscordURL = "https://discordapp.com/invite/EdrVAP5";
		private static readonly string ForumURL = "https://forum.unity.com/threads/best-tool-asset-store-award-amplify-shader-editor-node-based-shader-creation-tool.430959/";

		private static readonly string SiteURL = "http://amplify.pt/download/";
		private static readonly string ASPStoreURL = "https://assetstore.unity.com/packages/slug/202484";
		private static readonly string ASEStoreURL = "https://assetstore.unity.com/packages/tools/visual-scripting/amplify-shader-editor-68570?aid=1011lPwI&pubref=ShaderPack";
		private static readonly string BundleStoreURL = "https://assetstore.unity.com/packages/tools/visual-scripting/amplify-bundle-173849?aid=1011lPwI&pubref=ShaderPack";


		private static readonly GUIContent SamplesTitle = new GUIContent( "Shader Samples", "Import samples according to you project rendering pipeline" );
		private static readonly GUIContent ShaderCatalogueTitle = new GUIContent( "Shader Catalogue", "Check the online sample gallery of for each of the pipelines" );
		private static readonly GUIContent CommunityTitle = new GUIContent( "Community", "Need help? Reach us through our discord server or the offitial support Unity forum" );
		private static readonly GUIContent UpdateTitle = new GUIContent( "Latest Update", "Check the lastest additions, improvements and bug fixes done to ASP" );
		private static readonly GUIContent SRPTitle = new GUIContent( "SRP Version" , "Samples were created under a specific SRP version. Please check if yours is compatible." );
		private static readonly GUIContent ASPTitle = new GUIContent( "Amplify Shader Pack", "Are you using the latest version? Now you know" );
		private static readonly GUIContent[] ASPBody =
		{
			new GUIContent("All shaders created and fully editable with Amplify Shader Editor."),
			new GUIContent("Check out the Amplify Creations Unity bundle and save over $50!"),
			new GUIContent(" Amplify Bundle"),
			new GUIContent("Amplify Shader Editor"),
		};



		private static readonly string DownArrow = "\u25BC";
#if UNITY_2019_3_OR_NEWER
		private int DownButtonSize = 22;
#else
		private int DownButtonSize = 21;
#endif

		Vector2 m_scrollPosition = Vector2.zero;
		ASPPreferences.ShowOption m_startup = ASPPreferences.ShowOption.Never;
		bool m_showURP = false;
		bool m_showHDRP = false;

		[NonSerialized]
		Texture packageIcon = null;
		[NonSerialized]
		Texture textIcon = null;
		[NonSerialized]
		Texture webIcon = null;

		GUIContent BuiltInbutton = null;
		GUIContent HDRP10To11button = null;
		GUIContent HDRP7To9button = null;
		GUIContent URP10To11button = null;
		GUIContent URP7To9button = null;

		GUIContent HDRPCataloguebutton = null;
		GUIContent URPCatalogueButton = null;
		GUIContent Builtinbutton = null;


		GUIContent DiscordButton = null;
		GUIContent ForumButton = null;

		GUIContent ASEIcon = null;
		GUIContent ASPIcon = null;
		RenderTexture rt;

		[NonSerialized]
		GUIStyle m_buttonStyle = null;
		[NonSerialized]
		GUIStyle m_buttonLeftStyle = null;
		[NonSerialized]
		GUIStyle m_buttonRightStyle = null;
		[NonSerialized]
		GUIStyle m_minibuttonStyle = null;
		[NonSerialized]
		GUIStyle m_labelStyle = null;
		[NonSerialized]
		GUIStyle m_linkStyle = null;

		GUIStyle m_noMarginLinkStyle = null;

		private ChangeLogInfo m_changeLog;
		private bool m_infoDownloaded = false;
		private string m_newVersion = string.Empty;

		private static readonly string SRPErrorMessage = "An incompatible SRP version was found in this project, valid versions are 7.7.1 and 10.5.1. Please note that it doesn't necessarily mean that shaders will fail to compile but they may require being recompiled to this version using Amplify Shader Editor" ;
		private const string HDPackageId = "com.unity.render-pipelines.high-definition";
		private const string UniversalPackageId = "com.unity.render-pipelines.universal";

		private ListRequest m_packageListRequest = null;
		private bool m_requireUpdateList = false;
		private UnityEditor.PackageManager.PackageInfo m_urpPackageInfo;
		private bool m_urpCompatible = false;
		private UnityEditor.PackageManager.PackageInfo m_hdrpPackageInfo;
		private bool m_hdrpCompatible = false;
		private readonly Dictionary<string , bool> ValidVersions = new Dictionary<string, bool>
		{
			{ "7.7.1", true },
			{ "10.5.1", true }
		};

		void RequestPackageInfo()
		{
			if( !m_requireUpdateList )
			{
				m_requireUpdateList = true;
				m_packageListRequest = UnityEditor.PackageManager.Client.List( true );

				m_urpPackageInfo = null;
				m_urpCompatible = false;

				m_hdrpPackageInfo = null;
				m_hdrpCompatible = false;
			}
		}

		void CheckPackageRequest()
		{
			if( m_requireUpdateList )
			{
				if( m_packageListRequest != null && m_packageListRequest.IsCompleted )
				{
					m_requireUpdateList = false;
					foreach( UnityEditor.PackageManager.PackageInfo pi in m_packageListRequest.Result )
					{
						if( pi.name.Equals( UniversalPackageId ) )
						{
							m_urpPackageInfo = pi;
							m_urpCompatible = ValidVersions.ContainsKey( pi.version );
						}

						if( pi.name.Equals( HDPackageId ) )
						{
							m_hdrpPackageInfo = pi;
							m_hdrpCompatible = ValidVersions.ContainsKey( pi.version );
						}
					}
				}
			}
		}

		private void OnEnable()
		{
			rt = new RenderTexture( 16, 16, 0 );
			rt.Create();

			m_startup = (ASPPreferences.ShowOption)EditorPrefs.GetInt( ASPPreferences.PrefStartUp, 0 );

			if( textIcon == null )
			{
				Texture icon = EditorGUIUtility.IconContent( "TextAsset Icon" ).image;
				var cache = RenderTexture.active;
				RenderTexture.active = rt;
				Graphics.Blit( icon, rt );
				RenderTexture.active = cache;
				textIcon = rt;

				HDRPCataloguebutton = new GUIContent( " HDRP", textIcon );
				URPCatalogueButton = new GUIContent( " URP", textIcon );
				Builtinbutton = new GUIContent( " Built-in", textIcon );
				
			}

			if( packageIcon == null )
			{
				packageIcon = EditorGUIUtility.IconContent( "BuildSettings.Editor.Small" ).image;
				HDRP10To11button = new GUIContent( " HDRP Samples 10 to 11", packageIcon );
				HDRP7To9button = new GUIContent( " HDRP Samples 7 to 9" , packageIcon );
				URP10To11button = new GUIContent( " URP Samples 10 to 11", packageIcon );
				URP7To9button = new GUIContent( " URP Samples 7 to 9" , packageIcon );
				BuiltInbutton = new GUIContent( " Built-In Samples" , packageIcon );
			}

			if( webIcon == null )
			{
				webIcon = EditorGUIUtility.IconContent( "BuildSettings.Web.Small" ).image;
				DiscordButton = new GUIContent( " Discord", webIcon );
				ForumButton = new GUIContent( " Unity Forum", webIcon );
			}

			if( m_changeLog == null )
			{
				var changelog = AssetDatabase.LoadAssetAtPath<TextAsset>( AssetDatabase.GUIDToAssetPath( ChangeLogGUID ) );
				string lastUpdate = string.Empty;
				if( changelog != null )
				{
					lastUpdate = changelog.text;//changelog.text.Substring( 0, changelog.text.IndexOf( "\nv", 50 ) );// + "\n...";
					lastUpdate = lastUpdate.Replace( "    *" , "    \u25CB" );
					lastUpdate = lastUpdate.Replace( "* " , "\u2022 " );
				}
				m_changeLog = new ChangeLogInfo( VersionInfo.FullNumber , lastUpdate );
			}

			if( ASEIcon == null )
			{
				ASEIcon = new GUIContent( AssetDatabase.LoadAssetAtPath<Texture2D>( AssetDatabase.GUIDToAssetPath( ASEIconGUID ) ) );
			}

			if( ASPIcon == null )
			{
				ASPIcon = new GUIContent( AssetDatabase.LoadAssetAtPath<Texture2D>( AssetDatabase.GUIDToAssetPath( ASPIconGUID ) ) );
			}

			RequestPackageInfo();
		}

		private void OnDisable()
		{
			if( rt != null )
			{
				rt.Release();
				DestroyImmediate( rt );
			}
		}

		public void OnGUI()
		{
			CheckPackageRequest();
			if( !m_infoDownloaded )
			{
				m_infoDownloaded = true;

				StartBackgroundTask( StartRequest( ChangelogURL, () =>
				{
					var temp = ChangeLogInfo.CreateFromJSON( www.downloadHandler.text );
					if( temp != null && temp.Version >= m_changeLog.Version )
					{
						m_changeLog = temp;
					}
					// improve this later
					int major = m_changeLog.Version / 10000;
					int minor = ( m_changeLog.Version / 1000 ) - major * 10;
					int release = ( m_changeLog.Version / 100 ) - major * 100 - minor * 10;
					int revision = ( ( m_changeLog.Version / 10 ) - major * 1000 - minor * 100 - release * 10 ) + ( m_changeLog.Version - major * 10000 - minor * 1000 - release * 100 );
					m_newVersion = major + "." + minor + "." + release + "r" + revision;
					Repaint();
				} ) );
			}

			if( m_buttonStyle == null )
			{
				m_buttonStyle = new GUIStyle( GUI.skin.button );
				m_buttonStyle.alignment = TextAnchor.MiddleLeft;
			}

			if( m_buttonLeftStyle == null )
			{
				m_buttonLeftStyle = new GUIStyle( "ButtonLeft" );
				m_buttonLeftStyle.alignment = TextAnchor.MiddleLeft;
				m_buttonLeftStyle.margin = m_buttonStyle.margin;
				m_buttonLeftStyle.margin.right = 0;
			}

			if( m_buttonRightStyle == null )
			{
				m_buttonRightStyle = new GUIStyle( "ButtonRight" );
				m_buttonRightStyle.alignment = TextAnchor.MiddleLeft;
				m_buttonRightStyle.margin = m_buttonStyle.margin;
				m_buttonRightStyle.margin.left = 0;
			}

			if( m_minibuttonStyle == null )
			{
				m_minibuttonStyle = new GUIStyle( "MiniButton" );
				m_minibuttonStyle.alignment = TextAnchor.MiddleLeft;
				m_minibuttonStyle.margin = m_buttonStyle.margin;
				m_minibuttonStyle.margin.left = 20;
				m_minibuttonStyle.normal.textColor = m_buttonStyle.normal.textColor;
				m_minibuttonStyle.hover.textColor = m_buttonStyle.hover.textColor;
			}

			if( m_labelStyle == null )
			{
				m_labelStyle = new GUIStyle( "BoldLabel" );
				m_labelStyle.margin = new RectOffset( 4, 4, 4, 4 );
				m_labelStyle.padding = new RectOffset( 2, 2, 2, 2 );
				m_labelStyle.fontSize = 13;
			}

			if( m_linkStyle == null )
			{
				var inv = AssetDatabase.LoadAssetAtPath<Texture2D>( AssetDatabase.GUIDToAssetPath( "1004d06b4b28f5943abdf2313a22790a" ) ); // find a better solution for transparent buttons
				m_linkStyle = new GUIStyle();
				m_linkStyle.normal.textColor = new Color( 0.2980392f, 0.4901961f, 1f );
				m_linkStyle.hover.textColor = Color.white;
				m_linkStyle.active.textColor = Color.grey;
				m_linkStyle.margin.top = 3;
				m_linkStyle.margin.bottom = 2;
				m_linkStyle.hover.background = inv;
				m_linkStyle.active.background = inv;
			}

			if( m_noMarginLinkStyle == null )
			{
				m_noMarginLinkStyle = new GUIStyle( "BoldLabel" );
				m_noMarginLinkStyle.normal.textColor = new Color( 0.2980392f , 0.4901961f , 1f );
				m_noMarginLinkStyle.hover.textColor = Color.white;
				m_noMarginLinkStyle.active.textColor = Color.grey;
				m_noMarginLinkStyle.hover.background = m_linkStyle.hover.background;
				m_noMarginLinkStyle.active.background = m_linkStyle.active.background;
			}

			EditorGUILayout.BeginHorizontal( GUIStyle.none, GUILayout.ExpandWidth( true ) );
			{
				/////////////////////////////////////////////////////////////////////////////////////
				// LEFT COLUMN
				/////////////////////////////////////////////////////////////////////////////////////
				EditorGUILayout.BeginVertical( GUILayout.Width( 175 ) );
				{
					GUILayout.Label( SamplesTitle, m_labelStyle );
					EditorGUILayout.BeginHorizontal();
					if( GUILayout.Button( HDRP10To11button, m_buttonLeftStyle ) )
						ImportSample( HDRP10To11button.text, HDRPSamples10to11GUID );

					if( GUILayout.Button( DownArrow, m_buttonRightStyle, GUILayout.Width( DownButtonSize ), GUILayout.Height( DownButtonSize ) ) )
					{
						m_showHDRP = !m_showHDRP;
						m_showURP = false;
					}
					EditorGUILayout.EndHorizontal();
					if( m_showHDRP )
					{
						if( GUILayout.Button( HDRP7To9button , m_minibuttonStyle ) )
							ImportSample( HDRP7To9button.text , HDRPSamples7to9GUID );
					}

					EditorGUILayout.BeginHorizontal();
					if( GUILayout.Button( URP10To11button, m_buttonLeftStyle ) )
						ImportSample( URP10To11button.text, URPSamples10to11GUID );
					
					if( GUILayout.Button( DownArrow, m_buttonRightStyle, GUILayout.Width( DownButtonSize ), GUILayout.Height( DownButtonSize ) ) )
					{
						m_showURP = !m_showURP;
						m_showHDRP = false;
					}
					EditorGUILayout.EndHorizontal();
					if( m_showURP )
					{
						EditorGUILayout.BeginVertical();
						if( GUILayout.Button( URP7To9button, m_minibuttonStyle ) )
							ImportSample( URP7To9button.text, URPSamples7to9GUID );
						EditorGUILayout.EndVertical();
					}

					if( GUILayout.Button( BuiltInbutton , m_buttonStyle ) )
						ImportSample( BuiltInbutton.text , BiRPSamplesGUID );

					GUILayout.Space( 10 );

					GUILayout.Label( ShaderCatalogueTitle, m_labelStyle );
					if( GUILayout.Button( HDRPCataloguebutton, m_buttonStyle ) )
						Application.OpenURL( HDRPCaralogueURL );

					if( GUILayout.Button( URPCatalogueButton, m_buttonStyle ) )
						Application.OpenURL( URPCatalogueURL );

					if( GUILayout.Button( Builtinbutton, m_buttonStyle ) )
						Application.OpenURL( BuiltinCatalogueURL );
				}
				EditorGUILayout.EndVertical();
				
				/////////////////////////////////////////////////////////////////////////////////////
				// RIGHT COLUMN
				/////////////////////////////////////////////////////////////////////////////////////
				EditorGUILayout.BeginVertical( GUILayout.Width( 650 - 175 - 9 ), GUILayout.ExpandHeight( true ) );
				{
					GUILayout.Space( 20 );
					GUILayout.BeginHorizontal();
					{
						GUILayout.BeginVertical();
						{
							GUILayout.Label( ASPBody[ 0 ] );
							GUILayout.Label( ASPBody[ 1 ] );
							GUILayout.BeginHorizontal(GUILayout.Width(100));

							if( GUILayout.Button( ASPBody[ 2 ] , m_noMarginLinkStyle ) )
								Application.OpenURL( BundleStoreURL );

							GUILayout.Label( "-" );

							if( GUILayout.Button( ASPBody[ 3 ] , m_noMarginLinkStyle ) )
								Application.OpenURL( ASEStoreURL );

							GUILayout.EndHorizontal();
						}
						GUILayout.EndVertical();

						GUILayout.FlexibleSpace();
						EditorGUILayout.BeginVertical();
						{
							GUILayout.Space( 7 );
							GUILayout.Label( ASEIcon );
						}
						EditorGUILayout.EndVertical();
					}
					GUILayout.EndHorizontal();

					GUILayout.Space( 13 );

					//Discord/Forum
					//GUILayout.Label( CommunityTitle, m_labelStyle );
					GUILayout.BeginHorizontal( GUILayout.ExpandWidth( true ) );
					{
						if( GUILayout.Button( DiscordButton, GUILayout.ExpandWidth( true ) ) )
						{
							Application.OpenURL( DiscordURL );
						}
						if( GUILayout.Button( ForumButton, GUILayout.ExpandWidth( true ) ) )
						{
							Application.OpenURL( ForumURL );
						}
					}
					bool hasSRPPackage = false;
					Color bufferColor = GUI.color;
					GUILayout.EndHorizontal();
					GUILayout.Label( SRPTitle , m_labelStyle );
					GUILayout.BeginHorizontal( GUILayout.Width( 250 ) );
					{
						if( m_urpPackageInfo != null )
						{
							hasSRPPackage = true;
							GUI.color = m_urpCompatible ? Color.green:Color.red;
							GUILayout.Label( "URP version: " + m_urpPackageInfo.version );
						}

						if( m_hdrpPackageInfo != null )
						{
							hasSRPPackage = true;
							GUI.color = m_hdrpCompatible ? Color.green : Color.red;
							GUILayout.Label( "HDRP version: " + m_hdrpPackageInfo.version );
						}

						if( !hasSRPPackage )
						{
							GUILayout.Label( "No SRP version installed" );
						}
					}
					GUILayout.EndHorizontal();
					GUI.color = bufferColor;

					if( ( m_hdrpPackageInfo != null && !m_hdrpCompatible ) || ( m_urpPackageInfo != null && !m_urpCompatible ) )
					{
						EditorGUILayout.HelpBox( SRPErrorMessage , MessageType.Warning );
					}

					GUILayout.Label( UpdateTitle, m_labelStyle );
					m_scrollPosition = GUILayout.BeginScrollView( m_scrollPosition, "ProgressBarBack", GUILayout.ExpandHeight( true ), GUILayout.ExpandWidth( true ) );
					GUILayout.Label( m_changeLog.LastUpdate, "WordWrappedMiniLabel", GUILayout.ExpandHeight( true ) );

					GUILayout.EndScrollView();

					EditorGUILayout.BeginHorizontal( GUILayout.ExpandWidth( true ) );
					{
						EditorGUILayout.BeginVertical();
						{
							GUILayout.Label( ASPTitle , m_labelStyle );

							GUILayout.Label( "Installed Version: " + VersionInfo.StaticToString() );

							if( m_changeLog.Version > VersionInfo.FullNumber )
							{
								var cache = GUI.color;
								GUI.color = Color.red;
								GUILayout.Label( "New version available: " + m_newVersion , "BoldLabel" );
								GUI.color = cache;
							}
							else
							{
								var cache = GUI.color;
								GUI.color = Color.green;
								GUILayout.Label( "You are using the latest version" , "BoldLabel" );
								GUI.color = cache;
							}

							EditorGUILayout.BeginHorizontal();
							{
								GUILayout.Label( "Download links:" );
								if( GUILayout.Button( "Amplify" , m_linkStyle ) )
									Application.OpenURL( SiteURL );
								GUILayout.Label( "-" );
								if( GUILayout.Button( "Asset Store" , m_linkStyle ) )
									Application.OpenURL( ASPStoreURL );
							}
							EditorGUILayout.EndHorizontal();
							GUILayout.Space( 7 );
						}
						EditorGUILayout.EndVertical();

						GUILayout.FlexibleSpace();
						EditorGUILayout.BeginVertical();
						{
							GUILayout.Space( 7 );
							GUILayout.Label( ASPIcon );
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal( "ProjectBrowserBottomBarBg", GUILayout.ExpandWidth( true ), GUILayout.Height(22) );
			{
				GUILayout.FlexibleSpace();
				EditorGUI.BeginChangeCheck();
				var cache = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 100;
				m_startup = (ASPPreferences.ShowOption)EditorGUILayout.EnumPopup( "Show At Startup", m_startup, GUILayout.Width( 220 ) );
				EditorGUIUtility.labelWidth = cache;
				if( EditorGUI.EndChangeCheck() )
				{
					EditorPrefs.SetInt( ASPPreferences.PrefStartUp, (int)m_startup );
				}
			}
			EditorGUILayout.EndHorizontal();
			
			// Find a better way to update link buttons without repainting the window
			Repaint();
		}

		void ImportSample( string pipeline, string guid )
		{
			if( EditorUtility.DisplayDialog( "Import Sample", "This will import the samples for" + pipeline.Replace( " Samples", "" ) + ", please make sure the pipeline is properly installed and/or selected before importing the samples.\n\nContinue?", "Yes", "No" ) )
			{
				AssetDatabase.ImportPackage( AssetDatabase.GUIDToAssetPath( guid ), false );
			}
		}

		UnityWebRequest www;

		IEnumerator StartRequest( string url, Action success = null )
		{
			using( www = UnityWebRequest.Get( url ) )
			{
#if UNITY_2017_2_OR_NEWER
				yield return www.SendWebRequest();
#else
				yield return www.Send();
#endif

				while( www.isDone == false )
					yield return null;

				if( success != null )
					success();
			}
		}

		public static void StartBackgroundTask( IEnumerator update, Action end = null )
		{
			EditorApplication.CallbackFunction closureCallback = null;

			closureCallback = () =>
			{
				try
				{
					if( update.MoveNext() == false )
					{
						if( end != null )
							end();
						EditorApplication.update -= closureCallback;
					}
				}
				catch( Exception ex )
				{
					if( end != null )
						end();
					Debug.LogException( ex );
					EditorApplication.update -= closureCallback;
				}
			};

			EditorApplication.update += closureCallback;
		}
	}

	[Serializable]
	internal class ChangeLogInfo
	{
		public int Version;
		public string LastUpdate;

		public static ChangeLogInfo CreateFromJSON( string jsonString )
		{
			return JsonUtility.FromJson<ChangeLogInfo>( jsonString );
		}

		public ChangeLogInfo( int version, string lastUpdate )
		{
			Version = version;
			LastUpdate = lastUpdate;
		}
	}
}
