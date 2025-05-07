slider prefab
    click on value text shows input dialogue to manually set value
    validate within min/max bounds?
ripple func
    add boolean prop to make it round in X/Z
provide way to minimize controls
    either the whole thing or just the function props section
add image visualization
    file picker
        using SimpleFileBrowser;

        FileBrowser.ShowLoadDialog(
            paths =>
            {
                foreach (var p in paths)
                    Debug.Log(p);
            },
            () => Debug.Log("CANCEL"),
            FileBrowser.PickMode.Files
        );
