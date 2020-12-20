using Kasay.FodyHelpers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

internal class PropertyChangedWeaver
{
    readonly PropertyDefinition propertyDefinition;
    readonly TypeDefinition typeDefinition;
    FieldDefinition fieldDefinition;

    const String PREFIX = "<";
    const String SUFFIX = ">k__BackingField";

    public PropertyChangedWeaver(PropertyDefinition propertyDefinition)
    {
        this.propertyDefinition = propertyDefinition;
        typeDefinition = propertyDefinition.DeclaringType;

        SetFieldDefinition();
        ModifySetMethod();
    }

    void SetFieldDefinition()
    {
        fieldDefinition = typeDefinition
            .Fields
            .FirstOrDefault(_ => _.Name == GetBackingFieldName(propertyDefinition.Name));
    }

    void ModifySetMethod()
    {
        propertyDefinition.SetMethod.Body.Instructions.Clear();

        var instructions = propertyDefinition.SetMethod.Body.Instructions;
        var processor = propertyDefinition.SetMethod.Body.GetILProcessor();

        //var methodDefinition = typeDefinition
        //    .BaseType
        //    .Resolve()
        //    .Methods
        //    .SingleOrDefault(m => m.Name == "OnPropertyChanged");

        //var callSetProperty = typeDefinition
        //    .BaseType
        //    .Module
        //    .ImportReference(methodDefinition);

        var callSetProperty = typeDefinition
            .Module
            .GetMethodReference("Kasay.PropertyChanged.Notifiable", "OnPropertyChanged");

        processor.Emit(OpCodes.Nop);
        // if (_property != value)
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldfld, fieldDefinition);
        processor.Emit(OpCodes.Ldarg_1);
        processor.Emit(OpCodes.Ceq);

        processor.Emit(OpCodes.Ldc_I4_0);
        processor.Emit(OpCodes.Ceq);

        var indexIf = instructions.Count;

        processor.Emit(OpCodes.Nop);
        // _property = value;
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldarg_1);
        processor.Emit(OpCodes.Stfld, fieldDefinition);
        // NotifyPropertyChanged("Property");
        processor.Emit(OpCodes.Ldarg_0);
        processor.Emit(OpCodes.Ldstr, propertyDefinition.Name);
        processor.Emit(OpCodes.Callvirt, callSetProperty);

        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Nop);
        processor.Emit(OpCodes.Ret);

        var newInst = Instruction.Create(OpCodes.Brfalse_S, instructions[instructions.Count - 1]);

        processor.InsertAfter(instructions[indexIf], newInst);
    }

    String GetBackingFieldName(String propertyName)
    {
        return $"{PREFIX}{propertyName}{SUFFIX}";
    }  
}