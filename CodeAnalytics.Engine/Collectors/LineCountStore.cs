﻿using CodeAnalytics.Engine.Contracts.Collectors;
using CodeAnalytics.Engine.Contracts.Ids;

namespace CodeAnalytics.Engine.Collectors;

public sealed class LineCountStore
{
   public Dictionary<NodeId, LineCountNode> LineCountsPerNode { get; }
   public Dictionary<StringId, LineCountStats> LineCountsPerFile { get; }
   
   public LineCountStore(
      Dictionary<NodeId, LineCountNode>? perNode = null,
      Dictionary<StringId, LineCountStats>? perFile = null)
   {
      LineCountsPerNode = perNode ?? [];
      LineCountsPerFile = perFile ?? [];
   }
   
   public void AddToNode(NodeId nodeId, StringId file, LineCountStats lineCountStats)
   {
      if (!LineCountsPerNode.TryGetValue(nodeId, out var node))
      {
         node = LineCountsPerNode[nodeId] = new LineCountNode()
         {
            StatsPerFile = [],
            StatsPerProject = []
         };
      }
      
      node.StatsPerFile[file] =  lineCountStats;

      if (!node.StatsPerProject.TryGetValue(lineCountStats.ProjectId, out var project))
      {
         project = node.StatsPerProject[lineCountStats.ProjectId] = new LineCountStats()
         {
            ProjectId = lineCountStats.ProjectId,
            CodeCount = 0,
            LineCount = 0
         };
      }
      
      project.CodeCount += lineCountStats.CodeCount;
      project.LineCount += lineCountStats.LineCount;
   }

   public void Merge(LineCountStore source)
   {
      foreach (var (key, value) in source.LineCountsPerNode)
      {
         if (LineCountsPerNode.TryGetValue(key, out var node))
         {
            node.Merge(value);
            continue;
         }

         LineCountsPerNode[key] = value;
      }

      foreach (var (key, value) in source.LineCountsPerFile)
      {
         if (LineCountsPerFile.TryGetValue(key, out var file))
         {
            file.Merge(value);
            continue;
         }
         
         LineCountsPerFile[key] = value;
      }
   }
}