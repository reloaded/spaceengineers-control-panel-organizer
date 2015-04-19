
/// 
/// Iterates over all control panel groups and renames the terminal blocks within each one with 
/// the format of "<Singularized Group Name> [Script Hooks] <Auto Increment ID>" 
/// 
 
/// <summary> 
/// The format of the terminal blocks that are being renamed.</summary> 
public static String BLOCK_NAME_FORMAT = "%GROUP_NAME% %SCRIPT_HOOKS% %ID%"; 
 
void Main() 
{ 
	//List<IMyBlockGroup> groups = new List<IMyBlockGroup>(); 
	List<IMyBlockGroup> groups = GridTerminalSystem.BlockGroups; 
	 
	// groups.Add(new IMyBlockGroup("Station Reactors [Ingot/Uranium:P9:Split]", new List<IMyTerminalBlock>() { 
		// new IMyTerminalBlock("Station Reactor 1 [Ingot/Uranium:P9:Split]"), 
		// new IMyTerminalBlock("Station Reactors [Ingot/Uranium:P9:Split] 2"), 
		// new IMyTerminalBlock("Station Reactor 1 ") 
	// })); 
	 
	// groups.Add(new IMyBlockGroup("[Ingot/Iron:Split] Uranium Cargos", new List<IMyTerminalBlock>() { 
		// new IMyTerminalBlock("Cargo 1"), 
		// new IMyTerminalBlock("Cargo 2 [Ingot/Uranium]"), 
		// new IMyTerminalBlock("Cargo 3 []"), 
	// })); 
	 
	// groups.Add(new IMyBlockGroup("Assembler Computers", new List<IMyTerminalBlock>() { 
		// new IMyTerminalBlock("[Ore/Iron:P1,Ore/Cobalt:P1] Assembler Computer 1"), 
		// new IMyTerminalBlock("Assembler Computer"), 
		// new IMyTerminalBlock("Assembler Computer []"), 
	// })); 
  
	for(int i = 0; i < groups.Count; i++)  
	{      
		IMyBlockGroup group = groups[i];  
		 
		String groupName = group.Name.Trim(); 
		 
		// We always default the singularized group name to the unmodified version of the group name 
		String singularGroupName = groupName; 
		 
		ScriptHookParser groupScriptHook = new ScriptHookParser(groupName); 
		 
		// Console.WriteLine("Current Group: " + groupName); 
		 
		 
		//  
		// If the group name contains a custom script hook, the group hook will replace  
		// any existing hooks within each terminal block 
		// 
		// 
		// For example, the Group name "Station Reactors [Ingot/Uranium:P9:Split]" will have it's hook  
		// inserted into the name of the terminal blocks that are assigned to the group. 
		// 
		// If a terminal block has a name of "Station Reactor" it will become "Station Reactor [Ingot/Uranium:P9:Split]" 
		// If a terminal block has a name of "Station Reactor [Ingot/Uranium:P3]" it will become "Station Reactor [Ingot/Uranium:P9:Split]" 
		//             
		// Console.WriteLine("Group Has Hook: " + groupScriptHook.hasHook()); 
		if(groupScriptHook.hasHook()) 
		{ 
			// We need to extract the group name without the script hook so we check if the 
			// script hook is at the beginning or end of the group name. 
			if(groupScriptHook.getStartIndex() == 0) 
			{ 
				singularGroupName = groupName.Substring(groupScriptHook.getEndIndex()).Trim(); 
			} 
			// Else script hook was at the end 
			else 
			{ 
				singularGroupName = groupName.Substring(0, groupScriptHook.getStartIndex()).Trim(); 
			} 
			 
			// Console.WriteLine("Group Script Hook: " + groupScriptHook.getHook()); 
			// Console.WriteLine("Group Name w/o Hook: " + singularGroupName); 
		} 
		 
		 
		//  
		// Singularize the group name. This is very simple and error prone until SE injects Regex into the sandbox. 
		// 
		if(singularGroupName.EndsWith("ies"))  
		{  
			singularGroupName = singularGroupName.Substring(0, singularGroupName.Length - 3) + "y";  
		}  
		else if(singularGroupName.EndsWith("s"))  
		{  
			singularGroupName = singularGroupName.Substring(0, singularGroupName.Length - 1);  
		} 
		// Console.WriteLine("Singularized Group Name: " + singularGroupName); 
		 
		 
		// Iterate over each block assigned to this group and regenerate it's name 
		for(int j = 0; j < group.Blocks.Count; j++) 
		{ 
			IMyTerminalBlock block = group.Blocks[j]; 
			String blockDisplayName = block.DisplayNameText;  
			 
			// The new block name always defaults to the singularized group name 
			String blockName = singularGroupName; 
			 
			if(groupScriptHook.hasHook()) 
			{ 
				blockName += " " + groupScriptHook.getHook(); 
			} 
			else 
			{ 
				ScriptHookParser blockScriptHook = new ScriptHookParser(blockDisplayName); 
				 
				// Console.WriteLine("Block Has Hook: " + blockScriptHook.hasHook()); 
				if(blockScriptHook.hasHook()) 
				{ 
					blockName += " " + blockScriptHook.getHook(); 
					 
					// Console.WriteLine("Block Script Hook: " + blockScriptHook.getHook()); 
				} 
			} 
								   
			blockName += " " + (j + 1); 
			 
			block.SetCustomName(blockName); 
			 
			// Console.WriteLine("New Block Name: " + blockName); 
		} 
		 
		// Console.WriteLine(""); 
	} 
	 
} 
 
 
class ScriptHookParser 
{ 
	/// <summary> 
	/// The original unmodifed subject</summary> 
	private String subject; 
	 
	/// <summary> 
	/// The index of the script hook opening '['</summary> 
	private int startIndex; 
	 
	/// <summary> 
	/// The index of the script hook closing ']'</summary> 
	private int endIndex; 
	 
	/// <summary> 
	/// The script hook that was found.</summary> 
	private string hook; 
	 
	 
	/// <summary> 
	/// Parses the given string subject and searches for a script hook.</summary> 
	public ScriptHookParser(String subject) 
	{ 
		this.subject = subject; 
		 
		this.Parse(); 
	} 
	 
	protected void Parse() 
	{ 
		 
		this.startIndex = subject.IndexOf("["); 
		 
		if(startIndex == -1) 
		{ 
			return; 
		} 
		 
		this.endIndex = subject.IndexOf("]", this.startIndex) + 1; 
		 
		 
		if(this.startIndex > -1 && this.endIndex > -1) 
		{ 
			this.hook = subject.Substring(this.startIndex, this.endIndex - this.startIndex);  
		} 
	} 
	 
	public int getStartIndex() 
	{ 
		return this.startIndex; 
	} 
	 
	public int getEndIndex() 
	{ 
		return this.endIndex; 
	} 
	 
	public String getHook() 
	{ 
		return this.hook; 
	} 
	 
	public bool hasHook() 
	{ 
		return this.hook != null; 
	} 
}