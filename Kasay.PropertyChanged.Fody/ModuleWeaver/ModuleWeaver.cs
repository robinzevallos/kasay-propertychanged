using Fody;
using Kasay.FodyHelpers;
using Mono.Cecil;
using System.Collections.Generic;

public class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        SetOnPropertyChangedToTypes();
    }

    void SetOnPropertyChangedToTypes()
    {
        foreach (var type in ModuleDefinition.GetTypes())
        {         
            if (type.InheritFrom("Kasay.PropertyChanged.Notifiable"))
            {
                foreach (var property in type.Properties)
                {
                    if (property.ExistAttribute("NotifyAttribute"))
                    {
                        new PropertyChangedWeaver(property);
                    }
                }
            }
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }
}
