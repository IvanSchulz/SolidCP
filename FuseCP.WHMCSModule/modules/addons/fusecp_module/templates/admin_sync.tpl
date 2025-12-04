<h3>Sync automation</h3>
{if $settings.SyncActive eq 0}
    <div id="servicecontent_sync">
        <div class="errorbox">{$LANG.FuseCP_syncnotactive}</div>
    </div>
{else}
    <div id="servicecontent_sync"></div>
    <p>{$LANG.FuseCP_sync_nosetting}</p>
{/if}
