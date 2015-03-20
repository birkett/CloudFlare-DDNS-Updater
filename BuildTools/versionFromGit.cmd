git --version >nul 2>&1 && (
    echo Git found, source is at revision:
    for /f "delims=*" %%G in ('git rev-parse --short HEAD') do (@set SOURCEVERSION=%%G)
    git rev-parse --short HEAD
) || (
    echo Git not found.
    @set SOURCEVERSION=Unofficial
)

copy /y ..\..\BuildTools\GeneratedVersion.template ..\..\Properties\GeneratedVersion.cs

echo class GeneratedVersion { public const string szAdditionalVersion = "%SOURCEVERSION%"; } >> ../../Properties/GeneratedVersion.cs
exit 0
