# C# Solution/Project Analytics

**Turn your C# solutions into actionable and interesting insights.**  
CodeAnalytics.Engine parses .sln/.csproj structures, source files, references, and metrics, then exposes them for rich exploration in a Blazor-based Web Viewer. A lightweight console collector transforms your codebase into analyzable data, while the built‑in Source Browser lets you jump through files, symbols, and dependencies with ease.

> ⚠️ Early stage: APIs, storage formats, and UI pieces are still evolving - expect breaking changes.


## Supported features so far

- Fast Collector Console tool ⚡
   - Scan solutions/projects and emit normalized metadata + metrics 🧭
- Web Viewer of collected data 🖥️
   - Dashboard (coming) 📊
   - Source Explorer 📂
      - View Souce Code in nice highlighting 🎨
      - Navigate files and folders 📁
      - Find all occurrences of Symbols like classes, methods etc by clicking on them 🔍
## Coming up
- Search in Source Explorer 🔎 
- Metrics dashboard in the web viewer 📈
- Always more metrics ➕
- Advanced rules like ex: Warning for specific methods used in loops (DB Calls etc.) 🚨

## Example Screenshots sor far
![Source Explorer - Occurrences](Examples/Images/SourceExplorerOccurrences.png)
![Source Explorer - Folders](Examples/Images/SourceExplorerFolders.png)
