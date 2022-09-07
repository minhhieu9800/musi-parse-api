using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YoutubeAPI.Models.Related
{
    public partial class JsonModelRelated
    {
        [JsonProperty("contents")]
        public Contents Contents { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class Contents
    {
        [JsonProperty("twoColumnWatchNextResults")]
        public TwoColumnWatchNextResults TwoColumnWatchNextResults { get; set; }
    }

    public partial class TwoColumnWatchNextResults
    {
        [JsonProperty("secondaryResults")]
        public TwoColumnWatchNextResultsSecondaryResults SecondaryResults { get; set; }
    }

    public partial class TwoColumnWatchNextResultsSecondaryResults
    {
        [JsonProperty("secondaryResults")]
        public SecondaryResultsSecondaryResults SecondaryResults { get; set; }
    }

    public partial class SecondaryResultsSecondaryResults
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("targetId")]
        public string TargetId { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("compactAutoplayRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public CompactAutoplayRenderer CompactAutoplayRenderer { get; set; }

        [JsonProperty("compactVideoRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ResultCompactVideoRenderer CompactVideoRenderer { get; set; }

        [JsonProperty("compactRadioRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public CompactRadioRenderer CompactRadioRenderer { get; set; }

        [JsonProperty("continuationItemRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ContinuationItemRenderer ContinuationItemRenderer { get; set; }
    }

    public partial class CompactAutoplayRenderer
    {
        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("toggleDescription")]
        public InfoText ToggleDescription { get; set; }

        [JsonProperty("infoIcon")]
        public Icon InfoIcon { get; set; }

        [JsonProperty("infoText")]
        public InfoText InfoText { get; set; }

        [JsonProperty("contents")]
        public List<Content> Contents { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class Content
    {
        [JsonProperty("compactVideoRenderer")]
        public ContentCompactVideoRenderer CompactVideoRenderer { get; set; }
    }

    public partial class ContentCompactVideoRenderer
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("thumbnail")]
        public ChannelThumbnailClass Thumbnail { get; set; }

        [JsonProperty("title")]
        public LengthText Title { get; set; }

        [JsonProperty("longBylineText")]
        public BylineText LongBylineText { get; set; }

        [JsonProperty("publishedTimeText")]
        public Title PublishedTimeText { get; set; }

        [JsonProperty("viewCountText")]
        public Title ViewCountText { get; set; }

        [JsonProperty("lengthText")]
        public LengthText LengthText { get; set; }

        [JsonProperty("navigationEndpoint")]
        public CompactVideoRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("shortBylineText")]
        public BylineText ShortBylineText { get; set; }

        [JsonProperty("channelThumbnail")]
        public ChannelThumbnailClass ChannelThumbnail { get; set; }

        [JsonProperty("ownerBadges")]
        public List<OwnerBadge> OwnerBadges { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("shortViewCountText")]
        public Title ShortViewCountText { get; set; }

        [JsonProperty("menu")]
        public Menu Menu { get; set; }

        [JsonProperty("thumbnailOverlays")]
        public List<CompactVideoRendererThumbnailOverlay> ThumbnailOverlays { get; set; }

        [JsonProperty("accessibility")]
        public Accessibility Accessibility { get; set; }

        [JsonProperty("richThumbnail")]
        public RichThumbnail RichThumbnail { get; set; }
    }

    public partial class Accessibility
    {
        [JsonProperty("accessibilityData")]
        public AccessibilityData AccessibilityData { get; set; }
    }

    public partial class AccessibilityData
    {
        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public partial class ChannelThumbnailClass
    {
        [JsonProperty("thumbnails")]
        public List<ThumbnailElement> Thumbnails { get; set; }
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

    public partial class LengthText
    {
        [JsonProperty("accessibility")]
        public Accessibility Accessibility { get; set; }

        [JsonProperty("simpleText")]
        public string SimpleText { get; set; }
    }

    public partial class BylineText
    {
        [JsonProperty("runs")]
        public List<LongBylineTextRun> Runs { get; set; }
    }

    public partial class LongBylineTextRun
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("navigationEndpoint")]
        public RunNavigationEndpoint NavigationEndpoint { get; set; }
    }

    public partial class RunNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public NavigationEndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("browseEndpoint")]
        public BrowseEndpoint BrowseEndpoint { get; set; }
    }

    public partial class BrowseEndpoint
    {
        [JsonProperty("browseId")]
        public string BrowseId { get; set; }

        [JsonProperty("canonicalBaseUrl")]
        public string CanonicalBaseUrl { get; set; }
    }

    public partial class NavigationEndpointCommandMetadata
    {
        [JsonProperty("webCommandMetadata")]
        public PurpleWebCommandMetadata WebCommandMetadata { get; set; }
    }

    public partial class PurpleWebCommandMetadata
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        //[JsonProperty("webPageType")]
        //public WebPageType WebPageType { get; set; }

        [JsonProperty("rootVe")]
        public long RootVe { get; set; }

        //[JsonProperty("apiUrl", NullValueHandling = NullValueHandling.Ignore)]
        //public PurpleApiUrl? ApiUrl { get; set; }
    }

    public partial class Menu
    {
        [JsonProperty("menuRenderer")]
        public MenuRenderer MenuRenderer { get; set; }
    }

    public partial class MenuRenderer
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("accessibility")]
        public Accessibility Accessibility { get; set; }

        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetId { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("menuServiceItemRenderer")]
        public MenuServiceItemRenderer MenuServiceItemRenderer { get; set; }
    }

    public partial class MenuServiceItemRenderer
    {
        [JsonProperty("text")]
        public InfoText Text { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }

        [JsonProperty("serviceEndpoint")]
        public ServiceEndpoint ServiceEndpoint { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class Icon
    {
        //[JsonProperty("iconType")]
        //public IconType IconType { get; set; }
    }

    public partial class ServiceEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public ServiceEndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("signalServiceEndpoint")]
        public ServiceEndpointSignalServiceEndpoint SignalServiceEndpoint { get; set; }
    }

    public partial class ServiceEndpointCommandMetadata
    {
        [JsonProperty("webCommandMetadata")]
        public FluffyWebCommandMetadata WebCommandMetadata { get; set; }
    }

    public partial class FluffyWebCommandMetadata
    {
        [JsonProperty("sendPost")]
        public bool SendPost { get; set; }
    }

    public partial class ServiceEndpointSignalServiceEndpoint
    {
        //[JsonProperty("signal")]
        //public Signal Signal { get; set; }

        [JsonProperty("actions")]
        public List<PurpleAction> Actions { get; set; }
    }

    public partial class PurpleAction
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("addToPlaylistCommand", NullValueHandling = NullValueHandling.Ignore)]
        public AddToPlaylistCommand AddToPlaylistCommand { get; set; }

        [JsonProperty("openPopupAction", NullValueHandling = NullValueHandling.Ignore)]
        public OpenPopupAction OpenPopupAction { get; set; }
    }

    public partial class AddToPlaylistCommand
    {
        [JsonProperty("openMiniplayer")]
        public bool OpenMiniplayer { get; set; }

        [JsonProperty("openListPanel")]
        public bool OpenListPanel { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        //[JsonProperty("listType")]
        //public ListType ListType { get; set; }

        [JsonProperty("onCreateListCommand")]
        public OnCreateListCommand OnCreateListCommand { get; set; }

        [JsonProperty("videoIds")]
        public List<string> VideoIds { get; set; }
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
        public TentacledWebCommandMetadata WebCommandMetadata { get; set; }
    }

    public partial class TentacledWebCommandMetadata
    {
        [JsonProperty("sendPost")]
        public bool SendPost { get; set; }

        //[JsonProperty("apiUrl", NullValueHandling = NullValueHandling.Ignore)]
        //public FluffyApiUrl? ApiUrl { get; set; }
    }

    public partial class CreatePlaylistServiceEndpoint
    {
        [JsonProperty("videoIds")]
        public List<string> VideoIds { get; set; }

        //[JsonProperty("params")]
        //public Params Params { get; set; }
    }

    public partial class OpenPopupAction
    {
        [JsonProperty("popup")]
        public Popup Popup { get; set; }

        //[JsonProperty("popupType")]
        //public PopupType PopupType { get; set; }
    }

    public partial class Popup
    {
        [JsonProperty("notificationActionRenderer")]
        public NotificationActionRenderer NotificationActionRenderer { get; set; }
    }

    public partial class NotificationActionRenderer
    {
        [JsonProperty("responseText")]
        public Title ResponseText { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class Title
    {
        [JsonProperty("simpleText")]
        public string SimpleText { get; set; }
    }

    public partial class InfoText
    {
        [JsonProperty("runs")]
        public List<InfoTextRun> Runs { get; set; }
    }

    public partial class InfoTextRun
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class CompactVideoRendererNavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public NavigationEndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("watchEndpoint")]
        public PurpleWatchEndpoint WatchEndpoint { get; set; }
    }

    public partial class PurpleWatchEndpoint
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("nofollow")]
        public bool Nofollow { get; set; }
    }

    public partial class OwnerBadge
    {
        [JsonProperty("metadataBadgeRenderer")]
        public OwnerBadgeMetadataBadgeRenderer MetadataBadgeRenderer { get; set; }
    }

    public partial class OwnerBadgeMetadataBadgeRenderer
    {
        [JsonProperty("icon")]
        public Icon Icon { get; set; }

        //[JsonProperty("style")]
        //public MetadataBadgeRendererStyle Style { get; set; }

        //[JsonProperty("tooltip")]
        //public Tooltip Tooltip { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class RichThumbnail
    {
        [JsonProperty("movingThumbnailRenderer")]
        public MovingThumbnailRenderer MovingThumbnailRenderer { get; set; }
    }

    public partial class MovingThumbnailRenderer
    {
        [JsonProperty("movingThumbnailDetails", NullValueHandling = NullValueHandling.Ignore)]
        public MovingThumbnailDetails MovingThumbnailDetails { get; set; }

        [JsonProperty("enableHoveredLogging")]
        public bool EnableHoveredLogging { get; set; }

        [JsonProperty("enableOverlay")]
        public bool EnableOverlay { get; set; }
    }

    public partial class MovingThumbnailDetails
    {
        [JsonProperty("thumbnails")]
        public List<ThumbnailElement> Thumbnails { get; set; }

        [JsonProperty("logAsMovingThumbnail")]
        public bool LogAsMovingThumbnail { get; set; }
    }

    public partial class CompactVideoRendererThumbnailOverlay
    {
        [JsonProperty("thumbnailOverlayTimeStatusRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayTimeStatusRenderer ThumbnailOverlayTimeStatusRenderer { get; set; }

        [JsonProperty("thumbnailOverlayToggleButtonRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayToggleButtonRenderer ThumbnailOverlayToggleButtonRenderer { get; set; }

        [JsonProperty("thumbnailOverlayNowPlayingRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayNowPlayingRenderer ThumbnailOverlayNowPlayingRenderer { get; set; }
    }

    public partial class ThumbnailOverlayNowPlayingRenderer
    {
        [JsonProperty("text")]
        public InfoText Text { get; set; }
    }

    public partial class ThumbnailOverlayTimeStatusRenderer
    {
        [JsonProperty("text")]
        public LengthText Text { get; set; }

        //[JsonProperty("style")]
        //public ThumbnailOverlayTimeStatusRendererStyle Style { get; set; }
    }

    public partial class ThumbnailOverlayToggleButtonRenderer
    {
        [JsonProperty("isToggled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsToggled { get; set; }

        [JsonProperty("untoggledIcon")]
        public Icon UntoggledIcon { get; set; }

        [JsonProperty("toggledIcon")]
        public Icon ToggledIcon { get; set; }

        //[JsonProperty("untoggledTooltip")]
        //public UntoggledTooltip UntoggledTooltip { get; set; }

        //[JsonProperty("toggledTooltip")]
        //public ToggledTooltip ToggledTooltip { get; set; }

        [JsonProperty("untoggledServiceEndpoint")]
        public UntoggledServiceEndpoint UntoggledServiceEndpoint { get; set; }

        [JsonProperty("toggledServiceEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public ToggledServiceEndpoint ToggledServiceEndpoint { get; set; }

        [JsonProperty("untoggledAccessibility")]
        public Accessibility UntoggledAccessibility { get; set; }

        [JsonProperty("toggledAccessibility")]
        public Accessibility ToggledAccessibility { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class ToggledServiceEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public OnCreateListCommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("playlistEditEndpoint")]
        public ToggledServiceEndpointPlaylistEditEndpoint PlaylistEditEndpoint { get; set; }
    }

    public partial class ToggledServiceEndpointPlaylistEditEndpoint
    {
        //[JsonProperty("playlistId")]
        //public PlaylistId PlaylistId { get; set; }

        [JsonProperty("actions")]
        public List<FluffyAction> Actions { get; set; }
    }

    public partial class FluffyAction
    {
        //[JsonProperty("action")]
        //public IndigoAction Action { get; set; }

        [JsonProperty("removedVideoId")]
        public string RemovedVideoId { get; set; }
    }

    public partial class UntoggledServiceEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public OnCreateListCommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("playlistEditEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public UntoggledServiceEndpointPlaylistEditEndpoint PlaylistEditEndpoint { get; set; }

        [JsonProperty("signalServiceEndpoint", NullValueHandling = NullValueHandling.Ignore)]
        public UntoggledServiceEndpointSignalServiceEndpoint SignalServiceEndpoint { get; set; }
    }

    public partial class UntoggledServiceEndpointPlaylistEditEndpoint
    {
        //[JsonProperty("playlistId")]
        //public PlaylistId PlaylistId { get; set; }

        [JsonProperty("actions")]
        public List<TentacledAction> Actions { get; set; }
    }

    public partial class TentacledAction
    {
        [JsonProperty("addedVideoId")]
        public string AddedVideoId { get; set; }

        //[JsonProperty("action")]
        //public IndecentAction Action { get; set; }
    }

    public partial class UntoggledServiceEndpointSignalServiceEndpoint
    {
        //[JsonProperty("signal")]
        //public Signal Signal { get; set; }

        [JsonProperty("actions")]
        public List<StickyAction> Actions { get; set; }
    }

    public partial class StickyAction
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("addToPlaylistCommand")]
        public AddToPlaylistCommand AddToPlaylistCommand { get; set; }
    }

    public partial class CompactRadioRenderer
    {
        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("thumbnail")]
        public ChannelThumbnailClass Thumbnail { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("navigationEndpoint")]
        public NavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("videoCountText")]
        public InfoText VideoCountText { get; set; }

        [JsonProperty("secondaryNavigationEndpoint")]
        public NavigationEndpoint SecondaryNavigationEndpoint { get; set; }

        [JsonProperty("shortBylineText")]
        public Title ShortBylineText { get; set; }

        [JsonProperty("longBylineText")]
        public Title LongBylineText { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("thumbnailText")]
        public ThumbnailText ThumbnailText { get; set; }

        [JsonProperty("videoCountShortText")]
        public InfoText VideoCountShortText { get; set; }

        [JsonProperty("shareUrl")]
        public Uri ShareUrl { get; set; }

        [JsonProperty("thumbnailOverlays")]
        public List<CompactRadioRendererThumbnailOverlay> ThumbnailOverlays { get; set; }
    }

    public partial class NavigationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public NavigationEndpointCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("watchEndpoint")]
        public SecondaryNavigationEndpointWatchEndpoint WatchEndpoint { get; set; }
    }

    public partial class SecondaryNavigationEndpointWatchEndpoint
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("params")]
        public string Params { get; set; }

        [JsonProperty("continuePlayback", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ContinuePlayback { get; set; }
    }

    public partial class CompactRadioRendererThumbnailOverlay
    {
        [JsonProperty("thumbnailOverlaySidePanelRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayRenderer ThumbnailOverlaySidePanelRenderer { get; set; }

        [JsonProperty("thumbnailOverlayHoverTextRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayRenderer ThumbnailOverlayHoverTextRenderer { get; set; }

        [JsonProperty("thumbnailOverlayNowPlayingRenderer", NullValueHandling = NullValueHandling.Ignore)]
        public ThumbnailOverlayNowPlayingRenderer ThumbnailOverlayNowPlayingRenderer { get; set; }
    }

    public partial class ThumbnailOverlayRenderer
    {
        [JsonProperty("text")]
        public InfoText Text { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }
    }

    public partial class ThumbnailText
    {
        [JsonProperty("runs")]
        public List<ThumbnailTextRun> Runs { get; set; }
    }

    public partial class ThumbnailTextRun
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("bold", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Bold { get; set; }
    }

    public partial class ResultCompactVideoRenderer
    {
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        [JsonProperty("thumbnail")]
        public ChannelThumbnailClass Thumbnail { get; set; }

        [JsonProperty("title")]
        public LengthText Title { get; set; }

        [JsonProperty("longBylineText")]
        public BylineText LongBylineText { get; set; }

        [JsonProperty("publishedTimeText")]
        public Title PublishedTimeText { get; set; }

        [JsonProperty("viewCountText")]
        public Title ViewCountText { get; set; }

        [JsonProperty("lengthText")]
        public LengthText LengthText { get; set; }

        [JsonProperty("navigationEndpoint")]
        public CompactVideoRendererNavigationEndpoint NavigationEndpoint { get; set; }

        [JsonProperty("shortBylineText")]
        public BylineText ShortBylineText { get; set; }

        [JsonProperty("channelThumbnail")]
        public ChannelThumbnailClass ChannelThumbnail { get; set; }

        [JsonProperty("ownerBadges", NullValueHandling = NullValueHandling.Ignore)]
        public List<OwnerBadge> OwnerBadges { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("shortViewCountText")]
        public Title ShortViewCountText { get; set; }

        [JsonProperty("menu")]
        public Menu Menu { get; set; }

        [JsonProperty("thumbnailOverlays")]
        public List<CompactVideoRendererThumbnailOverlay> ThumbnailOverlays { get; set; }

        [JsonProperty("accessibility")]
        public Accessibility Accessibility { get; set; }

        [JsonProperty("richThumbnail")]
        public RichThumbnail RichThumbnail { get; set; }

        [JsonProperty("badges", NullValueHandling = NullValueHandling.Ignore)]
        public List<Badge> Badges { get; set; }
    }

    public partial class Badge
    {
        [JsonProperty("metadataBadgeRenderer")]
        public BadgeMetadataBadgeRenderer MetadataBadgeRenderer { get; set; }
    }

    public partial class BadgeMetadataBadgeRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }
    }

    public partial class ContinuationItemRenderer
    {
        [JsonProperty("trigger")]
        public string Trigger { get; set; }

        [JsonProperty("continuationEndpoint")]
        public ContinuationEndpoint ContinuationEndpoint { get; set; }

        [JsonProperty("button")]
        public Button Button { get; set; }
    }

    public partial class Button
    {
        [JsonProperty("buttonRenderer")]
        public ButtonRenderer ButtonRenderer { get; set; }
    }

    public partial class ButtonRenderer
    {
        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("text")]
        public InfoText Text { get; set; }

        [JsonProperty("trackingParams")]
        public string TrackingParams { get; set; }

        [JsonProperty("command")]
        public ContinuationEndpoint Command { get; set; }
    }

    public partial class ContinuationEndpoint
    {
        [JsonProperty("clickTrackingParams")]
        public string ClickTrackingParams { get; set; }

        [JsonProperty("commandMetadata")]
        public OnCreateListCommandCommandMetadata CommandMetadata { get; set; }

        [JsonProperty("continuationCommand")]
        public ContinuationCommand ContinuationCommand { get; set; }
    }

    public partial class ContinuationCommand
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }
    }

}
