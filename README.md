# Prefab Replacer for Unity
A tool to replace scene objects by prefabs in Unity

## Install ##

**Installation must be performed by project.**

Add the following line to your Packages/manifest.json file under dependencies:

    "com.ecasillas.prefabreplacer": "https://github.com/edcasillas/unity-prefab-replacer.git"
    
Open Unity again; the Package Manager will run and the package will be installed.

## Update ##

To ensure you have the latest version of the package, remove the version lock the Package Manager creates in Packages/manifest.json. The lock looks like this:

```
    "com.ecasillas.prefabreplacer": {
      "hash": "someValue",
      "revision": "HEAD"
    }
```