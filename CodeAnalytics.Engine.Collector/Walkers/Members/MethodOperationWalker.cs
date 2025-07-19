using CodeAnalytics.Engine.Collector.Collectors.Contexts;
using CodeAnalytics.Engine.Common.Buffers.Dynamic;
using CodeAnalytics.Engine.Contracts.Enums.Intermediate;
using CodeAnalytics.Engine.Contracts.Intermediate.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace CodeAnalytics.Engine.Collector.Walkers.Members;

public sealed class MethodOperationWalker : OperationWalker
{
   public ref PooledList<MemberUsageInfo> MemberUsages => ref _memberUsages;
   private PooledList<MemberUsageInfo> _memberUsages = [];
   
   private readonly CollectContext _context;
   private int _loopScore;

   public MethodOperationWalker(
      CollectContext context)
   {
      _context = context;
   }

   public override void VisitObjectCreation(IObjectCreationOperation operation)
   {
      if (operation.Constructor is not { OriginalDefinition: { } constructor })
      {
         base.VisitObjectCreation(operation);
         return;
      }
      
      var constId = _context.Store.NodeIdStore.GetOrAdd(constructor);
      var containingId = _context.Store.NodeIdStore.GetOrAdd(constructor.ContainingType);
      
      _memberUsages.Add(new MemberUsageInfo()
      {
         ContainingId = containingId,
         MemberId = constId,
         IsStatic = constructor.IsStatic,
         Type = MemberUsageType.Constructor,
         LoopScore = _loopScore,
      });
      
      base.VisitObjectCreation(operation);
   }

   public override void VisitPropertyReference(IPropertyReferenceOperation operation)
   {
      var property = operation.Property.OriginalDefinition;
      
      var propertyId = _context.Store.NodeIdStore.GetOrAdd(property);
      var containingId = _context.Store.NodeIdStore.GetOrAdd(property.ContainingType);
      
      _memberUsages.Add(new MemberUsageInfo()
      {
         ContainingId = containingId,
         MemberId = propertyId,
         IsStatic = property.IsStatic,
         Type = MemberUsageType.Property,
         LoopScore = _loopScore,
      });
      
      base.VisitPropertyReference(operation);
   }

   public override void VisitFieldReference(IFieldReferenceOperation operation)
   {
      var field = operation.Field.OriginalDefinition;
      
      var fieldId = _context.Store.NodeIdStore.GetOrAdd(field);
      var containingId = _context.Store.NodeIdStore.GetOrAdd(field.ContainingType);
      
      _memberUsages.Add(new MemberUsageInfo()
      {
         ContainingId = containingId,
         MemberId = fieldId,
         IsStatic = field.IsStatic,
         Type = MemberUsageType.Field,
         LoopScore = _loopScore,
      });
      
      base.VisitFieldReference(operation);
   }

   public override void VisitInvocation(IInvocationOperation operation)
   {
      var method = operation.TargetMethod.OriginalDefinition;
      
      var methodId = _context.Store.NodeIdStore.GetOrAdd(method);
      var containingId = _context.Store.NodeIdStore.GetOrAdd(method.ContainingType);
      
      _memberUsages.Add(new MemberUsageInfo()
      {
         ContainingId = containingId,
         MemberId = methodId,
         IsStatic = method.IsStatic,
         Type = MemberUsageType.MethodInvocation,
         LoopScore = _loopScore,
      });
      
      base.VisitInvocation(operation);
   }

   public override void VisitAnonymousFunction(IAnonymousFunctionOperation operation)
   {
      // special treatment in future maybe
      base.VisitAnonymousFunction(operation);
   }

   public override void VisitLocalFunction(ILocalFunctionOperation operation)
   {
      // special treatment in future maybe
      base.VisitLocalFunction(operation);
   }

   public override void VisitForLoop(IForLoopOperation operation)
   {
      _loopScore++;
      base.VisitForLoop(operation);
      _loopScore--;
   }

   public override void VisitWhileLoop(IWhileLoopOperation operation)
   {
      _loopScore++;
      base.VisitWhileLoop(operation);
      _loopScore--;
   }

   public override void VisitForEachLoop(IForEachLoopOperation operation)
   {
      _loopScore++;
      base.VisitForEachLoop(operation);
      _loopScore--;
   }

   public override void VisitForToLoop(IForToLoopOperation operation)
   {
      _loopScore++;
      base.VisitForToLoop(operation);
      _loopScore--;
   }
}