using System;
using System.Collections.Generic;

namespace YoutubeAPI.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Root
    {
        [JsonProperty("responseContext")]
        public ResponseContext ResponseContext { get; set; }

        [JsonProperty("contents")]
        public Contents Contents { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("topbar")]
        public Topbar Topbar { get; set; }

        [JsonProperty("microformat")]
        public Microformat Microformat { get; set; }

        [JsonProperty("sidebar")]
        public Sidebar Sidebar { get; set; }
    }

    public partial class Contents
    {
        [JsonProperty("twoColumnBrowseResultsRenderer")]
        public TwoColumnBrowseResultsRenderer TwoColumnBrowseResultsRenderer { get; set; }
    }

    public partial class TwoColumnBrowseResultsRenderer
    {
        [JsonProperty("tabs")]
        public Tab[] Tabs { get; set; }
    }

    public partial class Tab
    {
        [JsonProperty("tabRenderer")]
        public TabRenderer TabRenderer { get; set; }
    }

    public partial class TabRenderer
    {
        [JsonProperty("selected")]
        public bool Selected { get; set; }

        [JsonProperty("content")]
        public TabRendererContent Content { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class TabRendererContent
    {
        [JsonProperty("sectionListRenderer")]
        public SectionListRenderer SectionListRenderer { get; set; }
    }

    public partial class SectionListRenderer
    {
        [JsonProperty("contents")]
        public SectionListRendererContent[] Contents { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class SectionListRendererContent
    {
        [JsonProperty("itemSectionRenderer")]
        public ItemSectionRenderer ItemSectionRenderer { get; set; }
    }

    public partial class ItemSectionRenderer
    {
        [JsonProperty("contents")]
        public ItemSectionRendererContent[] Contents { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class ItemSectionRendererContent
    {
        [JsonProperty("playlistVideoListRenderer")]
        public PlaylistVideoListRenderer PlaylistVideoListRenderer { get; set; }
    }

    public partial class PlaylistVideoListRenderer
    {
        [JsonProperty("contents")]
        public PlaylistVideoListRendererContent[] Contents { get; set; }

        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("isEditable")]
        public bool IsEditable { get; set; }

        [JsonProperty("canReorder")]
        public bool CanReorder { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("targetId")]
        public string TargetId { get; set; }
    }

    public partial class PlaylistVideoListRendererContent
    {
        [JsonProperty("playlistVideoRenderer")]
        public PlaylistVideoRenderer PlaylistVideoRenderer { get; set; }
    }

    public partial class PlaylistVideoRenderer
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("thumbnail")]
        public PlaylistVideoRendererThumbnail Thumbnail { get; set; }

        [JsonProperty("title")]
        public PurpleTitle Title { get; set; }

        [JsonProperty("index")]
        public Description Index { get; set; }

        [JsonProperty("shortBylineText")]
        public ShortBylineTextClass ShortBylineText { get; set; }

        [JsonProperty("lengthText")]
        public LengthTextClass LengthText { get; set; }

        [JsonProperty("navigationEndpoint")]
        public PlaylistVideoRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("lengthSeconds")]
      
        public long LengthSeconds { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("isPlayable")]
        public bool IsPlayable { get; set; }

        [JsonProperty("menu")]
        public PlaylistVideoRendererMenu Menu { get; set; }

        [JsonProperty("thumbnailOverlays")]
        public PlaylistVideoRendererThumbnailOverlay[] ThumbnailOverlays { get; set; }
    }

    public partial class Description
    {
        [JsonProperty("simpleText")]
        public string SimpleText { get; set; }
    }

    public partial class LengthTextClass
    {
        [JsonProperty("accessibility")]
        public AccessibilityData Accessibility { get; set; }

        [JsonProperty("simpleText")]
        public string SimpleText { get; set; }
    }

    public partial class AccessibilityData
    {
        [JsonProperty("accessibilityData")]
        public Accessibility AccessibilityDataAccessibilityData { get; set; }
    }

    public partial class Accessibility
    {
        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public partial class PlaylistVideoRendererMenu
    {
        [JsonProperty("menuRenderer")]
        public PurpleMenuRenderer MenuRenderer { get; set; }
    }

    public partial class PurpleMenuRenderer
    {
        [JsonProperty("items")]
        public PurpleItem[] Items { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("accessibility")]
        public AccessibilityData Accessibility { get; set; }
    }

    public partial class PurpleItem
    {
        [JsonProperty("menuServiceItemRenderer")]
        public MenuServiceItemRenderer MenuServiceItemRenderer { get; set; }
    }

    public partial class MenuServiceItemRenderer
    {
        [JsonProperty("text")]
        public TitleClass Text { get; set; }

        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("serviceEndpoint")]
        public MenuServiceItemRendererServiceEndpoint ServiceEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class IconImage
    {
        [JsonProperty("iconType")]
        public string IconType { get; set; }
    }

    public partial class MenuServiceItemRendererServiceEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public CommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signalServiceEndpoint")]
        public PurpleSignalServiceEndpoint SignalServiceEndpoint { get; set; }
    }

    public partial class CommandCommandMetadata
    {
        [JsonProperty("webCommandMetadata")]
        public PurpleWebCommandMetadata WebCommandMetadata { get; set; }
    }

    public partial class PurpleWebCommandMetadata
    {
        [JsonProperty("sendPost")]
        public bool SendPost { get; set; }
    }

    public partial class PurpleSignalServiceEndpoint
    {
        [JsonProperty("signal")]
        public string Signal { get; set; }

        [JsonProperty("actions")]
        public PurpleAction[] Actions { get; set; }
    }

    public partial class PurpleAction
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("addToPlaylistCommand")]
        public AddToPlaylistCommand AddToPlaylistCommand { get; set; }
    }

    public partial class AddToPlaylistCommand
    {
        [JsonProperty("openMiniplayer")]
        public bool OpenMiniplayer { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("listType")]
        public string ListType { get; set; }

        [JsonProperty("onCreateListCommand")]
        public OnCreateListCommand OnCreateListCommand { get; set; }

        [JsonProperty("videoIds")]
        public string[] VideoIds { get; set; }
    }

    public partial class OnCreateListCommand
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public OnCreateListCommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("createPlaylistServiceEndpoint")]
        public CreatePlaylistServiceEndpoint CreatePlaylistServiceEndpoint { get; set; }
    }

    public partial class OnCreateListCommandCommandMetadata
    {
        [JsonProperty("webCommandMetadata")]
        public FluffyWebCommandMetadata WebCommandMetadata { get; set; }
    }

    public partial class FluffyWebCommandMetadata
    {
        [JsonProperty("sendPost")]
        public bool SendPost { get; set; }

        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }
    }

    public partial class CreatePlaylistServiceEndpoint
    {
        [JsonProperty("videoIds")]
        public string[] VideoIds { get; set; }

        [JsonProperty("params")]
        public string Params { get; set; }
    }

    public partial class TitleClass
    {
        [JsonProperty("runs")]
        public TextRun[] Runs { get; set; }
    }

    public partial class TextRun
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class PlaylistVideoRendererNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("watchEndpoint")]
        public WatchEndpoint WatchEndpoint { get; set; }
    }

    public partial class EndpointCommandMetadata
    {
        [JsonProperty("webCommandMetadata")]
        public TentacledWebCommandMetadata WebCommandMetadata { get; set; }
    }

    public partial class TentacledWebCommandMetadata
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("webPageType")]
        public string WebPageType { get; set; }

        [JsonProperty("rootVe")]
        public long RootVe { get; set; }

        [JsonProperty("apiUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ApiUrl { get; set; }
    }

    public partial class WatchEndpoint
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public long? Index { get; set; }

        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public string Params { get; set; }

        [JsonProperty("loggingContext")]
        public LoggingContext LoggingContext { get; set; }

        [JsonProperty("watchEndpointSupportedOnesieConfig")]
        public WatchEndpointSupportedOnesieConfig WatchEndpointSupportedOnesieConfig { get; set; }
    }

    public partial class LoggingContext
    {
        [JsonProperty("vssLoggingContext")]
        public VssLoggingContext VssLoggingContext { get; set; }
    }

    public partial class VssLoggingContext
    {
        [JsonProperty("serializedContextData")]
        public string SerializedContextData { get; set; }
    }

    public partial class WatchEndpointSupportedOnesieConfig
    {
        [JsonProperty("html5PlaybackOnesieConfig")]
        public Html5PlaybackOnesieConfig Html5PlaybackOnesieConfig { get; set; }
    }

    public partial class Html5PlaybackOnesieConfig
    {
        [JsonProperty("commonConfig")]
        public CommonConfig CommonConfig { get; set; }
    }

    public partial class CommonConfig
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class ShortBylineTextClass
    {
        [JsonProperty("runs")]
        public ShortBylineTextRun[] Runs { get; set; }
    }

    public partial class ShortBylineTextRun
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("navigationEndpoint")]
        public VideoOwnerRendererNavigationEndpoint NavigationEndpoint { get; set; }
    }

    public partial class VideoOwnerRendererNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("browseEndpoint")]
        public NavigationEndpointBrowseEndpoint BrowseEndpoint { get; set; }
    }

    public partial class NavigationEndpointBrowseEndpoint
    {
        [JsonProperty("browseId")]
        public string BrowseId { get; set; }

        [JsonProperty("canonicalBaseUrl")]
        public string CanonicalBaseUrl { get; set; }
    }

    public partial class PlaylistVideoRendererThumbnail
    {
        [JsonProperty("thumbnails")]
        public ThumbnailElement[] Thumbnails { get; set; }
    }

    public partial class ThumbnailElement
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

    public partial class PlaylistVideoRendererThumbnailOverlay
    {
        [JsonProperty("thumbnailOverlayTimeStatusRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayTimeStatusRenderer ThumbnailOverlayTimeStatusRenderer { get; set; }

        [JsonProperty("thumbnailOverlayNowPlayingRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayNowPlayingRenderer ThumbnailOverlayNowPlayingRenderer { get; set; }
    }

    public partial class ThumbnailOverlayNowPlayingRenderer
    {
        [JsonProperty("text")]
        public TitleClass Text { get; set; }
    }

    public partial class ThumbnailOverlayTimeStatusRenderer
    {
        [JsonProperty("text")]
        public LengthTextClass Text { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }
    }

    public partial class PurpleTitle
    {
        [JsonProperty("runs")]
        public TextRun[] Runs { get; set; }

        [JsonProperty("accessibility")]
        public AccessibilityData Accessibility { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("playlistMetadataRenderer")]
        public PlaylistMetadataRenderer PlaylistMetadataRenderer { get; set; }
    }

    public partial class PlaylistMetadataRenderer
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("androidAppindexingLink")]
        public string AndroidAppindexingLink { get; set; }

        [JsonProperty("iosAppindexingLink")]
        public string IosAppindexingLink { get; set; }
    }

    public partial class Microformat
    {
        [JsonProperty("microformatDataRenderer")]
        public MicroformatDataRenderer MicroformatDataRenderer { get; set; }
    }

    public partial class MicroformatDataRenderer
    {
        [JsonProperty("urlCanonical")]
        public string UrlCanonical { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("thumbnail")]
        public PlaylistVideoRendererThumbnail Thumbnail { get; set; }

        [JsonProperty("siteName")]
        public string SiteName { get; set; }

        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("androidPackage")]
        public string AndroidPackage { get; set; }

        [JsonProperty("iosAppStoreId")]
       
        public long IosAppStoreId { get; set; }

        [JsonProperty("iosAppArguments")]
        public Uri IosAppArguments { get; set; }

        [JsonProperty("ogType")]
        public string OgType { get; set; }

        [JsonProperty("urlApplinksWeb")]
        public string UrlApplinksWeb { get; set; }

        [JsonProperty("urlApplinksIos")]
        public string UrlApplinksIos { get; set; }

        [JsonProperty("urlApplinksAndroid")]
        public string UrlApplinksAndroid { get; set; }

        [JsonProperty("urlTwitterIos")]
        public string UrlTwitterIos { get; set; }

        [JsonProperty("urlTwitterAndroid")]
        public string UrlTwitterAndroid { get; set; }

        [JsonProperty("twitterCardType")]
        public string TwitterCardType { get; set; }

        [JsonProperty("twitterSiteHandle")]
        public string TwitterSiteHandle { get; set; }

        [JsonProperty("schemaDotOrgType")]
        public Uri SchemaDotOrgType { get; set; }

        [JsonProperty("noindex")]
        public bool Noindex { get; set; }

        [JsonProperty("unlisted")]
        public bool Unlisted { get; set; }

        [JsonProperty("linkAlternates")]
        public LinkAlternate[] LinkAlternates { get; set; }
    }

    public partial class LinkAlternate
    {
        [JsonProperty("hrefUrl")]
        public string HrefUrl { get; set; }
    }

    public partial class ResponseContext
    {
        [JsonProperty("serviceTrackingParams")]
        public ServiceTrackingParam[] ServiceTrackingParams { get; set; }

        [JsonProperty("mainAppWebResponseContext")]
        public MainAppWebResponseContext MainAppWebResponseContext { get; set; }

        [JsonProperty("webResponseContextExtensionData")]
        public WebResponseContextExtensionData WebResponseContextExtensionData { get; set; }
    }

    public partial class MainAppWebResponseContext
    {
        [JsonProperty("loggedOut")]
        public bool LoggedOut { get; set; }
    }

    public partial class ServiceTrackingParam
    {
        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("params")]
        public Param[] Params { get; set; }
    }

    public partial class Param
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class WebResponseContextExtensionData
    {
        [JsonProperty("ytConfigData")]
        public YtConfigData YtConfigData { get; set; }

        [JsonProperty("hasDecorated")]
        public bool HasDecorated { get; set; }
    }

    public partial class YtConfigData
    {
        [JsonProperty("visitorData")]
        public string VisitorData { get; set; }

        [JsonProperty("rootVisualElementType")]
        public long RootVisualElementType { get; set; }
    }

    public partial class Sidebar
    {
        [JsonProperty("playlistSidebarRenderer")]
        public PlaylistSidebarRenderer PlaylistSidebarRenderer { get; set; }
    }

    public partial class PlaylistSidebarRenderer
    {
        [JsonProperty("items")]
        public PlaylistSidebarRendererItem[] Items { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class PlaylistSidebarRendererItem
    {
        [JsonProperty("playlistSidebarPrimaryInfoRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public PlaylistSidebarPrimaryInfoRenderer PlaylistSidebarPrimaryInfoRenderer { get; set; }

        [JsonProperty("playlistSidebarSecondaryInfoRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public PlaylistSidebarSecondaryInfoRenderer PlaylistSidebarSecondaryInfoRenderer { get; set; }
    }

    public partial class PlaylistSidebarPrimaryInfoRenderer
    {
        [JsonProperty("thumbnailRenderer")]
        public ThumbnailRenderer ThumbnailRenderer { get; set; }

        [JsonProperty("title")]
        public PlaylistSidebarPrimaryInfoRendererTitle Title { get; set; }

        [JsonProperty("stats")]
        public Stat[] Stats { get; set; }

        [JsonProperty("menu")]
        public PlaylistSidebarPrimaryInfoRendererMenu Menu { get; set; }

        [JsonProperty("thumbnailOverlays")]
        public PlaylistSidebarPrimaryInfoRendererThumbnailOverlay[] ThumbnailOverlays { get; set; }

        [JsonProperty("navigationEndpoint")]
        public PlaylistVideoRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("showMoreText")]
        public TitleClass ShowMoreText { get; set; }
    }

    public partial class PlaylistSidebarPrimaryInfoRendererMenu
    {
        [JsonProperty("menuRenderer")]
        public FluffyMenuRenderer MenuRenderer { get; set; }
    }

    public partial class FluffyMenuRenderer
    {
        [JsonProperty("items")]
        public FluffyItem[] Items { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("topLevelButtons")]
        public TopLevelButton[] TopLevelButtons { get; set; }

        [JsonProperty("accessibility")]
        public AccessibilityData Accessibility { get; set; }
    }

    public partial class FluffyItem
    {
        [JsonProperty("menuNavigationItemRenderer")]
        public MenuNavigationItemRenderer MenuNavigationItemRenderer { get; set; }
    }

    public partial class MenuNavigationItemRenderer
    {
        [JsonProperty("text")]
        public Description Text { get; set; }

        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("navigationEndpoint")]
        public MenuNavigationItemRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class MenuNavigationItemRendererNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public DefaultNavigationEndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("modalEndpoint")]
        public PurpleModalEndpoint ModalEndpoint { get; set; }
    }

    public partial class DefaultNavigationEndpointCommandMetadata
    {
        [JsonProperty("webCommandMetadata")]
        public StickyWebCommandMetadata WebCommandMetadata { get; set; }
    }

    public partial class StickyWebCommandMetadata
    {
        [JsonProperty("ignoreNavigation")]
        public bool IgnoreNavigation { get; set; }
    }

    public partial class PurpleModalEndpoint
    {
        [JsonProperty("modal")]
        public PurpleModal Modal { get; set; }
    }

    public partial class PurpleModal
    {
        [JsonProperty("modalWithTitleAndButtonRenderer")]
        public PurpleModalWithTitleAndButtonRenderer ModalWithTitleAndButtonRenderer { get; set; }
    }

    public partial class PurpleModalWithTitleAndButtonRenderer
    {
        [JsonProperty("title")]
        public Description Title { get; set; }

        [JsonProperty("content")]
        public Description Content { get; set; }

        [JsonProperty("button")]
        public A11YSkipNavigationButtonClass Button { get; set; }
    }

    public partial class A11YSkipNavigationButtonClass
    {
        [JsonProperty("buttonRenderer")]
        public A11YSkipNavigationButtonButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class A11YSkipNavigationButtonButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("text")]
        public TitleClass Text { get; set; }

        [JsonProperty("navigationEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public ButtonRendererCommand Command { get; set; }
    }

    public partial class ButtonRendererCommand
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public CommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signalServiceEndpoint")]
        public CommandSignalServiceEndpoint SignalServiceEndpoint { get; set; }
    }

    public partial class CommandSignalServiceEndpoint
    {
        [JsonProperty("signal")]
        public string Signal { get; set; }

        [JsonProperty("actions")]
        public FluffyAction[] Actions { get; set; }
    }

    public partial class FluffyAction
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("signalAction")]
        public SignalAction SignalAction { get; set; }
    }

    public partial class SignalAction
    {
        [JsonProperty("signal")]
        public string Signal { get; set; }
    }

    public partial class PurpleNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signInEndpoint")]
        public PurpleSignInEndpoint SignInEndpoint { get; set; }
    }

    public partial class PurpleSignInEndpoint
    {
        [JsonProperty("nextEndpoint")]
        public Endpoint NextEndpoint { get; set; }
    }

    public partial class Endpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("browseEndpoint")]
        public EndpointBrowseEndpoint BrowseEndpoint { get; set; }
    }

    public partial class EndpointBrowseEndpoint
    {
        [JsonProperty("browseId")]
        public string BrowseId { get; set; }
    }

    public partial class TopLevelButton
    {
        [JsonProperty("toggleButtonRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ToggleButtonRenderer ToggleButtonRenderer { get; set; }

        [JsonProperty("buttonRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public TopLevelButtonButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class TopLevelButtonButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("navigationEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public PlaylistVideoRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("accessibility")]
        public Accessibility Accessibility { get; set; }

        [JsonProperty("tooltip")]
        public string Tooltip { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("serviceEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleServiceEndpoint ServiceEndpoint { get; set; }
    }

    public partial class PurpleServiceEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public OnCreateListCommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("shareEntityServiceEndpoint")]
        public ShareEntityServiceEndpoint ShareEntityServiceEndpoint { get; set; }
    }

    public partial class ShareEntityServiceEndpoint
    {
        [JsonProperty("serializedShareEntity")]
        public string SerializedShareEntity { get; set; }

        [JsonProperty("commands")]
        public CommandElement[] Commands { get; set; }
    }

    public partial class CommandElement
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("openPopupAction")]
        public CommandOpenPopupAction OpenPopupAction { get; set; }
    }

    public partial class CommandOpenPopupAction
    {
        [JsonProperty("popup")]
        public PurplePopup Popup { get; set; }

        [JsonProperty("popupType")]
        public string PopupType { get; set; }

        [JsonProperty("beReused")]
        public bool BeReused { get; set; }
    }

    public partial class PurplePopup
    {
        [JsonProperty("unifiedSharePanelRenderer")]
        public UnifiedSharePanelRenderer UnifiedSharePanelRenderer { get; set; }
    }

    public partial class UnifiedSharePanelRenderer
    {
        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("showLoadingSpinner")]
        public bool ShowLoadingSpinner { get; set; }
    }

    public partial class ToggleButtonRenderer
    {
        [JsonProperty("style")]
        public StyleClass Style { get; set; }

        [JsonProperty("size")]
        public Size Size { get; set; }

        [JsonProperty("isToggled")]
        public bool IsToggled { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("defaultIcon")]
        public IconImage DefaultIcon { get; set; }

        [JsonProperty("toggledIcon")]
        public IconImage ToggledIcon { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("defaultTooltip")]
        public string DefaultTooltip { get; set; }

        [JsonProperty("toggledTooltip")]
        public string ToggledTooltip { get; set; }

        [JsonProperty("defaultNavigationEndpoint")]
        public DefaultNavigationEndpoint DefaultNavigationEndpoint { get; set; }

        [JsonProperty("accessibilityData")]
        public AccessibilityData AccessibilityData { get; set; }

        [JsonProperty("toggledAccessibilityData")]
        public AccessibilityData ToggledAccessibilityData { get; set; }
    }

    public partial class DefaultNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public DefaultNavigationEndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("modalEndpoint")]
        public DefaultNavigationEndpointModalEndpoint ModalEndpoint { get; set; }
    }

    public partial class DefaultNavigationEndpointModalEndpoint
    {
        [JsonProperty("modal")]
        public FluffyModal Modal { get; set; }
    }

    public partial class FluffyModal
    {
        [JsonProperty("modalWithTitleAndButtonRenderer")]
        public FluffyModalWithTitleAndButtonRenderer ModalWithTitleAndButtonRenderer { get; set; }
    }

    public partial class FluffyModalWithTitleAndButtonRenderer
    {
        [JsonProperty("title")]
        public Description Title { get; set; }

        [JsonProperty("content")]
        public Description Content { get; set; }

        [JsonProperty("button")]
        public PurpleButton Button { get; set; }
    }

    public partial class PurpleButton
    {
        [JsonProperty("buttonRenderer")]
        public PurpleButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class PurpleButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("text")]
        public Description Text { get; set; }

        [JsonProperty("navigationEndpoint")]
        public FluffyNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class FluffyNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signInEndpoint")]
        public FluffySignInEndpoint SignInEndpoint { get; set; }
    }

    public partial class FluffySignInEndpoint
    {
        [JsonProperty("nextEndpoint")]
        public Endpoint NextEndpoint { get; set; }

        [JsonProperty("idamTag")]
        
        public long IdamTag { get; set; }
    }

    public partial class Size
    {
        [JsonProperty("sizeType")]
        public string SizeType { get; set; }
    }

    public partial class StyleClass
    {
        [JsonProperty("styleType")]
        public string StyleType { get; set; }
    }

    public partial class Stat
    {
        [JsonProperty("runs", NullValueHandling = NullValueHandling.Ignore)]
        public TextRun[] Runs { get; set; }

        [JsonProperty("simpleText", NullValueHandling = NullValueHandling.Ignore)]
        public string SimpleText { get; set; }
    }

    public partial class PlaylistSidebarPrimaryInfoRendererThumbnailOverlay
    {
        [JsonProperty("thumbnailOverlaySidePanelRenderer")]
        public ThumbnailOverlaySidePanelRenderer ThumbnailOverlaySidePanelRenderer { get; set; }
    }

    public partial class ThumbnailOverlaySidePanelRenderer
    {
        [JsonProperty("text")]
        public Description Text { get; set; }

        [JsonProperty("icon")]
        public IconImage Icon { get; set; }
    }

    public partial class ThumbnailRenderer
    {
        [JsonProperty("playlistVideoThumbnailRenderer")]
        public PlaylistVideoThumbnailRenderer PlaylistVideoThumbnailRenderer { get; set; }
    }

    public partial class PlaylistVideoThumbnailRenderer
    {
        [JsonProperty("thumbnail")]
        public PlaylistVideoRendererThumbnail Thumbnail { get; set; }
    }

    public partial class PlaylistSidebarPrimaryInfoRendererTitle
    {
        [JsonProperty("runs")]
        public PurpleRun[] Runs { get; set; }
    }

    public partial class PurpleRun
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("navigationEndpoint")]
        public PlaylistVideoRendererNavigationEndpoint NavigationEndpoint { get; set; }
    }

    public partial class PlaylistSidebarSecondaryInfoRenderer
    {
        [JsonProperty("videoOwner")]
        public VideoOwner VideoOwner { get; set; }

        [JsonProperty("button")]
        public DismissButtonClass Button { get; set; }
    }

    public partial class DismissButtonClass
    {
        [JsonProperty("buttonRenderer")]
        public DismissButtonButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class DismissButtonButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("text")]
        public TitleClass Text { get; set; }

        [JsonProperty("navigationEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public TentacledNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class TentacledNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public DefaultNavigationEndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("modalEndpoint")]
        public FluffyModalEndpoint ModalEndpoint { get; set; }
    }

    public partial class FluffyModalEndpoint
    {
        [JsonProperty("modal")]
        public TentacledModal Modal { get; set; }
    }

    public partial class TentacledModal
    {
        [JsonProperty("modalWithTitleAndButtonRenderer")]
        public TentacledModalWithTitleAndButtonRenderer ModalWithTitleAndButtonRenderer { get; set; }
    }

    public partial class TentacledModalWithTitleAndButtonRenderer
    {
        [JsonProperty("title")]
        public Description Title { get; set; }

        [JsonProperty("content")]
        public Description Content { get; set; }

        [JsonProperty("button")]
        public FluffyButton Button { get; set; }
    }

    public partial class FluffyButton
    {
        [JsonProperty("buttonRenderer")]
        public FluffyButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class FluffyButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("text")]
        public Description Text { get; set; }

        [JsonProperty("navigationEndpoint")]
        public StickyNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class StickyNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signInEndpoint")]
        public TentacledSignInEndpoint SignInEndpoint { get; set; }
    }

    public partial class TentacledSignInEndpoint
    {
        [JsonProperty("nextEndpoint")]
        public Endpoint NextEndpoint { get; set; }

        [JsonProperty("continueAction")]
        public string ContinueAction { get; set; }

        [JsonProperty("idamTag")]
       
        public long IdamTag { get; set; }
    }

    public partial class VideoOwner
    {
        [JsonProperty("videoOwnerRenderer")]
        public VideoOwnerRenderer VideoOwnerRenderer { get; set; }
    }

    public partial class VideoOwnerRenderer
    {
        [JsonProperty("thumbnail")]
        public PlaylistVideoRendererThumbnail Thumbnail { get; set; }

        [JsonProperty("title")]
        public ShortBylineTextClass Title { get; set; }

        [JsonProperty("navigationEndpoint")]
        public VideoOwnerRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class Topbar
    {
        [JsonProperty("desktopTopbarRenderer")]
        public DesktopTopbarRenderer DesktopTopbarRenderer { get; set; }
    }

    public partial class DesktopTopbarRenderer
    {
        [JsonProperty("logo")]
        public Logo Logo { get; set; }

        [JsonProperty("searchbox")]
        public Searchbox Searchbox { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("topbarButtons")]
        public TopbarButton[] TopbarButtons { get; set; }

        [JsonProperty("hotkeyDialog")]
        public HotkeyDialog HotkeyDialog { get; set; }

        [JsonProperty("backButton")]
        public BackButtonClass BackButton { get; set; }

        [JsonProperty("forwardButton")]
        public BackButtonClass ForwardButton { get; set; }

        [JsonProperty("a11ySkipNavigationButton")]
        public A11YSkipNavigationButtonClass A11YSkipNavigationButton { get; set; }

        [JsonProperty("voiceSearchButton")]
        public VoiceSearchButtonClass VoiceSearchButton { get; set; }
    }

    public partial class BackButtonClass
    {
        [JsonProperty("buttonRenderer")]
        public BackButtonButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class BackButtonButtonRenderer
    {
        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("command")]
        public ButtonRendererCommand Command { get; set; }
    }

    public partial class HotkeyDialog
    {
        [JsonProperty("hotkeyDialogRenderer")]
        public HotkeyDialogRenderer HotkeyDialogRenderer { get; set; }
    }

    public partial class HotkeyDialogRenderer
    {
        [JsonProperty("title")]
        public TitleClass Title { get; set; }

        [JsonProperty("sections")]
        public HotkeyDialogRendererSection[] Sections { get; set; }

        [JsonProperty("dismissButton")]
        public DismissButtonClass DismissButton { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class HotkeyDialogRendererSection
    {
        [JsonProperty("hotkeyDialogSectionRenderer")]
        public HotkeyDialogSectionRenderer HotkeyDialogSectionRenderer { get; set; }
    }

    public partial class HotkeyDialogSectionRenderer
    {
        [JsonProperty("title")]
        public TitleClass Title { get; set; }

        [JsonProperty("options")]
        public Option[] Options { get; set; }
    }

    public partial class Option
    {
        [JsonProperty("hotkeyDialogSectionOptionRenderer")]
        public HotkeyDialogSectionOptionRenderer HotkeyDialogSectionOptionRenderer { get; set; }
    }

    public partial class HotkeyDialogSectionOptionRenderer
    {
        [JsonProperty("label")]
        public TitleClass Label { get; set; }

        [JsonProperty("hotkey")]
        public string Hotkey { get; set; }

        [JsonProperty("hotkeyAccessibilityLabel", NullValueHandling = NullValueHandling.Ignore)]
        public AccessibilityData HotkeyAccessibilityLabel { get; set; }
    }

    public partial class Logo
    {
        [JsonProperty("topbarLogoRenderer")]
        public TopbarLogoRenderer TopbarLogoRenderer { get; set; }
    }

    public partial class TopbarLogoRenderer
    {
        [JsonProperty("iconImage")]
        public IconImage IconImage { get; set; }

        [JsonProperty("tooltipText")]
        public TitleClass TooltipText { get; set; }

        [JsonProperty("endpoint")]
        public Endpoint Endpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("overrideEntityKey")]
        public string OverrideEntityKey { get; set; }
    }

    public partial class Searchbox
    {
        [JsonProperty("fusionSearchboxRenderer")]
        public FusionSearchboxRenderer FusionSearchboxRenderer { get; set; }
    }

    public partial class FusionSearchboxRenderer
    {
        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("placeholderText")]
        public TitleClass PlaceholderText { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("searchEndpoint")]
        public FusionSearchboxRendererSearchEndpoint SearchEndpoint { get; set; }

        [JsonProperty("clearButton")]
        public VoiceSearchButtonClass ClearButton { get; set; }
    }

    public partial class VoiceSearchDialogRenderer
    {
        [JsonProperty("placeholderHeader")]
        public TitleClass PlaceholderHeader { get; set; }

        [JsonProperty("promptHeader")]
        public TitleClass PromptHeader { get; set; }

        [JsonProperty("exampleQuery1")]
        public TitleClass ExampleQuery1 { get; set; }

        [JsonProperty("exampleQuery2")]
        public TitleClass ExampleQuery2 { get; set; }

        [JsonProperty("promptMicrophoneLabel")]
        public TitleClass PromptMicrophoneLabel { get; set; }

        [JsonProperty("loadingHeader")]
        public TitleClass LoadingHeader { get; set; }

        [JsonProperty("connectionErrorHeader")]
        public TitleClass ConnectionErrorHeader { get; set; }

        [JsonProperty("connectionErrorMicrophoneLabel")]
        public TitleClass ConnectionErrorMicrophoneLabel { get; set; }

        [JsonProperty("permissionsHeader")]
        public TitleClass PermissionsHeader { get; set; }

        [JsonProperty("permissionsSubtext")]
        public TitleClass PermissionsSubtext { get; set; }

        [JsonProperty("disabledHeader")]
        public TitleClass DisabledHeader { get; set; }

        [JsonProperty("disabledSubtext")]
        public TitleClass DisabledSubtext { get; set; }

        [JsonProperty("microphoneButtonAriaLabel")]
        public TitleClass MicrophoneButtonAriaLabel { get; set; }

        [JsonProperty("exitButton")]
        public VoiceSearchButtonClass ExitButton { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("microphoneOffPromptHeader")]
        public TitleClass MicrophoneOffPromptHeader { get; set; }
    }

    public partial class FluffyPopup
    {
        [JsonProperty("voiceSearchDialogRenderer")]
        public VoiceSearchDialogRenderer VoiceSearchDialogRenderer { get; set; }
    }

    public partial class PurpleOpenPopupAction
    {
        [JsonProperty("popup")]
        public FluffyPopup Popup { get; set; }

        [JsonProperty("popupType")]
        public string PopupType { get; set; }
    }

    public partial class TentacledAction
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("openPopupAction")]
        public PurpleOpenPopupAction OpenPopupAction { get; set; }
    }

    public partial class FluffySignalServiceEndpoint
    {
        [JsonProperty("signal")]
        public string Signal { get; set; }

        [JsonProperty("actions")]
        public TentacledAction[] Actions { get; set; }
    }

    public partial class FluffyServiceEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public CommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signalServiceEndpoint")]
        public FluffySignalServiceEndpoint SignalServiceEndpoint { get; set; }
    }

    public partial class VoiceSearchButtonButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("accessibilityData")]
        public AccessibilityData AccessibilityData { get; set; }

        [JsonProperty("serviceEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public FluffyServiceEndpoint ServiceEndpoint { get; set; }

        [JsonProperty("tooltip", NullValueHandling = NullValueHandling.Ignore)]
        public string Tooltip { get; set; }
    }

    public partial class VoiceSearchButtonClass
    {
        [JsonProperty("buttonRenderer")]
        public VoiceSearchButtonButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class Config
    {
        [JsonProperty("webSearchboxConfig")]
        public WebSearchboxConfig WebSearchboxConfig { get; set; }
    }

    public partial class WebSearchboxConfig
    {
        [JsonProperty("requestLanguage")]
        public string RequestLanguage { get; set; }

        [JsonProperty("requestDomain")]
        public string RequestDomain { get; set; }

        [JsonProperty("hasOnscreenKeyboard")]
        public bool HasOnscreenKeyboard { get; set; }

        [JsonProperty("focusSearchbox")]
        public bool FocusSearchbox { get; set; }
    }

    public partial class FusionSearchboxRendererSearchEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("searchEndpoint")]
        public SearchEndpointSearchEndpoint SearchEndpoint { get; set; }
    }

    public partial class SearchEndpointSearchEndpoint
    {
        [JsonProperty("query")]
        public string Query { get; set; }
    }

    public partial class TopbarButton
    {
        [JsonProperty("topbarMenuButtonRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public TopbarMenuButtonRenderer TopbarMenuButtonRenderer { get; set; }

        [JsonProperty("buttonRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public TopbarButtonButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class TopbarButtonButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("text")]
        public TitleClass Text { get; set; }

        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("navigationEndpoint")]
        public IndigoNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("targetId")]
        public string TargetId { get; set; }
    }

    public partial class IndigoNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signInEndpoint")]
        public StickySignInEndpoint SignInEndpoint { get; set; }
    }

    public partial class StickySignInEndpoint
    {
        [JsonProperty("idamTag")]
       
        public long IdamTag { get; set; }
    }

    public partial class TopbarMenuButtonRenderer
    {
        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("menuRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public TopbarMenuButtonRendererMenuRenderer MenuRenderer { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("accessibility")]
        public AccessibilityData Accessibility { get; set; }

        [JsonProperty("tooltip")]
        public string Tooltip { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetId { get; set; }

        [JsonProperty("menuRequest", NullValueHandling = NullValueHandling.Ignore)]
        public MenuRequest MenuRequest { get; set; }
    }

    public partial class TopbarMenuButtonRendererMenuRenderer
    {
        [JsonProperty("multiPageMenuRenderer")]
        public MenuRendererMultiPageMenuRenderer MultiPageMenuRenderer { get; set; }
    }

    public partial class MenuRendererMultiPageMenuRenderer
    {
        [JsonProperty("sections")]
        public MultiPageMenuRendererSection[] Sections { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }
    }

    public partial class MultiPageMenuRendererSection
    {
        [JsonProperty("multiPageMenuSectionRenderer")]
        public MultiPageMenuSectionRenderer MultiPageMenuSectionRenderer { get; set; }
    }

    public partial class MultiPageMenuSectionRenderer
    {
        [JsonProperty("items")]
        public MultiPageMenuSectionRendererItem[] Items { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class MultiPageMenuSectionRendererItem
    {
        [JsonProperty("compactLinkRenderer")]
        public CompactLinkRenderer CompactLinkRenderer { get; set; }
    }

    public partial class CompactLinkRenderer
    {
        [JsonProperty("icon")]
        public IconImage Icon { get; set; }

        [JsonProperty("title")]
        public TitleClass Title { get; set; }

        [JsonProperty("navigationEndpoint")]
        public CompactLinkRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class CompactLinkRendererNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public EndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("urlEndpoint")]
        public UrlEndpoint UrlEndpoint { get; set; }
    }

    public partial class UrlEndpoint
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }
    }

    public partial class MenuRequest
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public OnCreateListCommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signalServiceEndpoint")]
        public MenuRequestSignalServiceEndpoint SignalServiceEndpoint { get; set; }
    }

    public partial class MenuRequestSignalServiceEndpoint
    {
        [JsonProperty("signal")]
        public string Signal { get; set; }

        [JsonProperty("actions")]
        public StickyAction[] Actions { get; set; }
    }

    public partial class StickyAction
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("openPopupAction")]
        public FluffyOpenPopupAction OpenPopupAction { get; set; }
    }

    public partial class FluffyOpenPopupAction
    {
        [JsonProperty("popup")]
        public TentacledPopup Popup { get; set; }

        [JsonProperty("popupType")]
        public string PopupType { get; set; }

        [JsonProperty("beReused")]
        public bool BeReused { get; set; }
    }

    public partial class TentacledPopup
    {
        [JsonProperty("multiPageMenuRenderer")]
        public PopupMultiPageMenuRenderer MultiPageMenuRenderer { get; set; }
    }

    public partial class PopupMultiPageMenuRenderer
    {
        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("showLoadingSpinner")]
        public bool ShowLoadingSpinner { get; set; }
    }

    }
