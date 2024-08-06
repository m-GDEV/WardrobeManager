let SessionLoad = 1
if &cp | set nocp | endif
let s:so_save = &g:so | let s:siso_save = &g:siso | setg so=0 siso=0 | setl so=-1 siso=-1
let v:this_session=expand("<sfile>:p")
silent only
silent tabonly
cd /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager
if expand('%') == '' && !&modified && line('$') <= 1 && getline(1) == ''
  let s:wipebuf = bufnr('%')
endif
let s:shortmess_save = &shortmess
if &shortmess =~ 'A'
  set shortmess=aoOA
else
  set shortmess=aoO
endif
badd +4 f.html
badd +18 WardrobeManager.Presentation/Pages/Public/Index.razor
badd +9 WardrobeManager.Presentation/App.razor
badd +12 WardrobeManager.Presentation/_Imports.razor
badd +4 WardrobeManager.Shared/Misc/Constants.cs
badd +61 WardrobeManager.Presentation/Layout/NavMenu.razor
badd +3 WardrobeManager.Shared/Exceptions/UserNotFoundException.cs
badd +37 WardrobeManager.Api/Program.cs
badd +15 WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
badd +1 WardrobeManager.Api/Database/Services/Implementation/UserService.cs
badd +13 WardrobeManager.Shared/Models/NewOrEditedClothingItem.cs
badd +37 WardrobeManager.Shared/Models/ServerClothingItem.cs
badd +17 WardrobeManager.Shared/Models/User.cs
badd +1 WardrobeManager.Shared/Enums/Season.cs
badd +11 WardrobeManager.Api/Database/Services/Interfaces/IUserService.cs
badd +12 WardrobeManager.Shared/Services/Implementation/SharedService.cs
badd +8 WardrobeManager.Api/Database/DatabaseContext.cs
badd +1 WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs
badd +21 WardrobeManager.Presentation/Pages/Authenticated/Clothing.razor
badd +42 WardrobeManager.Presentation/Componenets/AddClothingItem.razor
badd +43 WardrobeManager.Presentation/wwwroot/index.html
badd +1 WardrobeManager.Presentation/Layout/MainLayout.razor
badd +1 WardrobeManager.Shared/Enums/ClothingCategory.cs
badd +2 WardrobeManager.Presentation/wwwroot/css/custom.css
badd +0 NERD_tree_4
badd +0 WardrobeManager.Presentation/Componenets/FormItems/SwitchToggle.razor
badd +0 WardrobeManager.Presentation/Componenets/Forms/AddClothingItem.razor
badd +0 WardrobeManager.Presentation/Componenets/Forms/EditClothingItem.razor
argglobal
%argdel
$argadd f.html
set stal=2
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabrewind
edit WardrobeManager.Api/Program.cs
let s:save_splitbelow = &splitbelow
let s:save_splitright = &splitright
set splitbelow splitright
wincmd _ | wincmd |
vsplit
1wincmd h
wincmd w
wincmd _ | wincmd |
split
wincmd _ | wincmd |
split
2wincmd k
wincmd w
wincmd w
let &splitbelow = s:save_splitbelow
let &splitright = s:save_splitright
wincmd t
let s:save_winminheight = &winminheight
let s:save_winminwidth = &winminwidth
set winminheight=0
set winheight=1
set winminwidth=0
set winwidth=1
exe '1resize ' . ((&lines * 45 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 99 + 104) / 209)
exe '2resize ' . ((&lines * 14 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 109 + 104) / 209)
exe '3resize ' . ((&lines * 13 + 25) / 50)
exe 'vert 3resize ' . ((&columns * 109 + 104) / 209)
exe '4resize ' . ((&lines * 16 + 25) / 50)
exe 'vert 4resize ' . ((&columns * 109 + 104) / 209)
argglobal
balt WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 37 - ((21 * winheight(0) + 22) / 45)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 37
normal! 029|
wincmd w
argglobal
if bufexists(fnamemodify("WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs", ":p")) | buffer WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs | else | edit WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs | endif
balt WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 37 - ((5 * winheight(0) + 7) / 14)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 37
normal! 09|
wincmd w
argglobal
if bufexists(fnamemodify("WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs", ":p")) | buffer WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs | else | edit WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs | endif
balt WardrobeManager.Api/Database/Services/Interfaces/IUserService.cs
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 7 - ((2 * winheight(0) + 6) / 13)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 7
normal! 0
lcd /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager
wincmd w
argglobal
if bufexists(fnamemodify("/mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Api/Database/Services/Interfaces/IUserService.cs", ":p")) | buffer /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Api/Database/Services/Interfaces/IUserService.cs | else | edit /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Api/Database/Services/Interfaces/IUserService.cs | endif
balt /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 13 - ((12 * winheight(0) + 8) / 16)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 13
normal! 0
wincmd w
exe '1resize ' . ((&lines * 45 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 99 + 104) / 209)
exe '2resize ' . ((&lines * 14 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 109 + 104) / 209)
exe '3resize ' . ((&lines * 13 + 25) / 50)
exe 'vert 3resize ' . ((&columns * 109 + 104) / 209)
exe '4resize ' . ((&lines * 16 + 25) / 50)
exe 'vert 4resize ' . ((&columns * 109 + 104) / 209)
tabnext
edit /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Pages/Authenticated/Clothing.razor
argglobal
balt /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Shared/Enums/ClothingCategory.cs
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 40 - ((22 * winheight(0) + 22) / 45)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 40
normal! 0
tabnext
edit /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/FormItems/SwitchToggle.razor
let s:save_splitbelow = &splitbelow
let s:save_splitright = &splitright
set splitbelow splitright
wincmd _ | wincmd |
vsplit
1wincmd h
wincmd _ | wincmd |
split
1wincmd k
wincmd w
wincmd w
let &splitbelow = s:save_splitbelow
let &splitright = s:save_splitright
wincmd t
let s:save_winminheight = &winminheight
let s:save_winminwidth = &winminwidth
set winminheight=0
set winheight=1
set winminwidth=0
set winwidth=1
exe '1resize ' . ((&lines * 22 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 104 + 104) / 209)
exe '2resize ' . ((&lines * 24 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 104 + 104) / 209)
exe 'vert 3resize ' . ((&columns * 104 + 104) / 209)
argglobal
balt /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/Forms/EditClothingItem.razor
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 11 - ((10 * winheight(0) + 11) / 22)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 11
normal! 0
wincmd w
argglobal
if bufexists(fnamemodify("/mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/Forms/EditClothingItem.razor", ":p")) | buffer /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/Forms/EditClothingItem.razor | else | edit /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/Forms/EditClothingItem.razor | endif
balt /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/FormItems/SwitchToggle.razor
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 28 - ((1 * winheight(0) + 12) / 24)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 28
normal! 05|
wincmd w
argglobal
if bufexists(fnamemodify("/mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/Forms/AddClothingItem.razor", ":p")) | buffer /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/Forms/AddClothingItem.razor | else | edit /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/Forms/AddClothingItem.razor | endif
balt /mnt/FILEZ/Files/Documents/code/GITCLONE/my-stuff/WardrobeManager/WardrobeManager.Presentation/Componenets/FormItems/SwitchToggle.razor
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
silent! normal! zE
let &fdl = &fdl
let s:l = 35 - ((34 * winheight(0) + 23) / 47)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 35
normal! 0
wincmd w
exe '1resize ' . ((&lines * 22 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 104 + 104) / 209)
exe '2resize ' . ((&lines * 24 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 104 + 104) / 209)
exe 'vert 3resize ' . ((&columns * 104 + 104) / 209)
tabnext 3
set stal=1
if exists('s:wipebuf') && len(win_findbuf(s:wipebuf)) == 0
  silent exe 'bwipe ' . s:wipebuf
endif
unlet! s:wipebuf
set winheight=1 winwidth=20
let &shortmess = s:shortmess_save
let &winminheight = s:save_winminheight
let &winminwidth = s:save_winminwidth
let s:sx = expand("<sfile>:p:r")."x.vim"
if filereadable(s:sx)
  exe "source " . fnameescape(s:sx)
endif
let &g:so = s:so_save | let &g:siso = s:siso_save
doautoall SessionLoadPost
unlet SessionLoad
" vim: set ft=vim :
