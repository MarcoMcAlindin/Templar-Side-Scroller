var MobilePlugin = {
     IsMobile: function()
     {
         return Module.SystemInfo.mobile;
     }
 };
 
 mergeInto(LibraryManager.library, MobilePlugin);