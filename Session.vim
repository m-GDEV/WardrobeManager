let SessionLoad = 1
if &cp | set nocp | endif
let s:so_save = &g:so | let s:siso_save = &g:siso | setg so=0 siso=0 | setl so=-1 siso=-1
let v:this_session=expand("<sfile>:p")
silent only
silent tabonly
cd /mnt/d/Windows\ Shit/Visual\ Studio\ Projects/WardrobeManager
if expand('%') == '' && !&modified && line('$') <= 1 && getline(1) == ''
  let s:wipebuf = bufnr('%')
endif
let s:shortmess_save = &shortmess
if &shortmess =~ 'A'
  set shortmess=aoOA
else
  set shortmess=aoO
endif
badd +1 WardrobeManager.Api/Program.cs
badd +13 WardrobeManager.Api/Endpoints/ClothingEndpoints.cs
badd +44 WardrobeManager.Api/Database/Services/Implementation/UserService.cs
badd +11 WardrobeManager.Api/Database/Services/Interfaces/IUserService.cs
badd +24 WardrobeManager.Shared/Services/Implementation/SharedService.cs
badd +23 WardrobeManager.Api/Endpoints/ImageEndpoints.cs
badd +47 WardrobeManager.Api/Database/Services/Implementation/FileService.cs
badd +9 WardrobeManager.Api/Database/Services/Interfaces/IFileService.cs
badd +23 WardrobeManager.Api/Endpoints/MiscEndpoints.cs
badd +55 WardrobeManager.Shared/Models/ServerClothingItem.cs
badd +2 WardrobeManager.Shared/Models/NewOrEditedClothingItem.cs
badd +1 WardrobeManager.Shared/Models/NewClothingItemDTO.cs
badd +2 WardrobeManager.Shared/Models/ClientClothingItem.cs
badd +5 WardrobeManager.Shared/Models/User.cs
badd +4 WardrobeManager.Shared/Enums/WearLocation.cs
badd +9 WardrobeManager.Shared/Services/Interfaces/ISharedService.cs
badd +33 WardrobeManager.Api/Endpoints/ActionEndpoints.cs
badd +119 WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
badd +7 WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs
badd +10 WardrobeManager.Shared/Exceptions/Exceptions.cs
badd +0 WardrobeManager.Presentation/Componenets/Forms/AddClothingItem.razor
badd +11 WardrobeManager.Shared/Misc/Constants.cs
badd +10 ~/.vimrc
badd +25 ~/.vim/coc-settings.json
badd +11 WardrobeManager.Shared/Enums/ClothingCategory.cs
badd +6 WardrobeManager.Shared/Enums/Season.cs
badd +0 WardrobeManager.Presentation/App.razor
badd +0 WardrobeManager.Presentation/wwwroot/index.html
argglobal
%argdel
set stal=2
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabrewind
edit WardrobeManager.Api/Program.cs
argglobal
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
let s:l = 129 - ((15 * winheight(0) + 15) / 31)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 129
normal! 0
tabnext
edit WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
argglobal
balt WardrobeManager.Api/Endpoints/ActionEndpoints.cs
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
let s:l = 119 - ((8 * winheight(0) + 15) / 31)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 119
normal! 076|
tabnext
edit WardrobeManager.Presentation/Componenets/Forms/AddClothingItem.razor
argglobal
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
let s:l = 46 - ((19 * winheight(0) + 15) / 31)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 46
normal! 064|
tabnext
edit WardrobeManager.Presentation/App.razor
argglobal
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
let s:l = 10 - ((9 * winheight(0) + 15) / 31)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 10
normal! 09|
tabnext
edit WardrobeManager.Presentation/wwwroot/index.html
argglobal
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
let s:l = 46 - ((5 * winheight(0) + 15) / 31)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 46
normal! 09|
tabnext
edit WardrobeManager.Api/Endpoints/ClothingEndpoints.cs
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
wincmd _ | wincmd |
split
1wincmd k
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
exe '1resize ' . ((&lines * 5 + 17) / 34)
exe 'vert 1resize ' . ((&columns * 34 + 45) / 90)
exe '2resize ' . ((&lines * 25 + 17) / 34)
exe 'vert 2resize ' . ((&columns * 34 + 45) / 90)
exe '3resize ' . ((&lines * 10 + 17) / 34)
exe 'vert 3resize ' . ((&columns * 55 + 45) / 90)
exe '4resize ' . ((&lines * 20 + 17) / 34)
exe 'vert 4resize ' . ((&columns * 55 + 45) / 90)
argglobal
balt WardrobeManager.Api/Database/Services/Implementation/UserService.cs
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
let s:l = 13 - ((2 * winheight(0) + 2) / 5)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 13
normal! 0
wincmd w
argglobal
if bufexists(fnamemodify("WardrobeManager.Api/Database/Services/Implementation/UserService.cs", ":p")) | buffer WardrobeManager.Api/Database/Services/Implementation/UserService.cs | else | edit WardrobeManager.Api/Database/Services/Implementation/UserService.cs | endif
balt WardrobeManager.Api/Endpoints/ClothingEndpoints.cs
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
let s:l = 44 - ((7 * winheight(0) + 12) / 25)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 44
normal! 09|
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
let s:l = 24 - ((6 * winheight(0) + 5) / 10)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 24
normal! 0
wincmd w
argglobal
if bufexists(fnamemodify("WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs", ":p")) | buffer WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs | else | edit WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs | endif
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
let s:l = 7 - ((4 * winheight(0) + 10) / 20)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 7
normal! 05|
wincmd w
exe '1resize ' . ((&lines * 5 + 17) / 34)
exe 'vert 1resize ' . ((&columns * 34 + 45) / 90)
exe '2resize ' . ((&lines * 25 + 17) / 34)
exe 'vert 2resize ' . ((&columns * 34 + 45) / 90)
exe '3resize ' . ((&lines * 10 + 17) / 34)
exe 'vert 3resize ' . ((&columns * 55 + 45) / 90)
exe '4resize ' . ((&lines * 20 + 17) / 34)
exe 'vert 4resize ' . ((&columns * 55 + 45) / 90)
tabnext
edit WardrobeManager.Shared/Models/User.cs
argglobal
balt WardrobeManager.Shared/Models/NewClothingItemDTO.cs
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
let s:l = 24 - ((15 * winheight(0) + 15) / 31)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 24
normal! 054|
tabnext
edit WardrobeManager.Shared/Exceptions/Exceptions.cs
let s:save_splitbelow = &splitbelow
let s:save_splitright = &splitright
set splitbelow splitright
wincmd _ | wincmd |
vsplit
1wincmd h
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
exe '1resize ' . ((&lines * 16 + 17) / 34)
exe 'vert 1resize ' . ((&columns * 64 + 45) / 90)
exe '2resize ' . ((&lines * 16 + 17) / 34)
exe 'vert 2resize ' . ((&columns * 55 + 45) / 90)
argglobal
enew
balt WardrobeManager.Shared/Exceptions/Exceptions.cs
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal nofen
wincmd w
argglobal
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
let s:l = 10 - ((3 * winheight(0) + 8) / 16)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 10
normal! 0
wincmd w
exe '1resize ' . ((&lines * 16 + 17) / 34)
exe 'vert 1resize ' . ((&columns * 64 + 45) / 90)
exe '2resize ' . ((&lines * 16 + 17) / 34)
exe 'vert 2resize ' . ((&columns * 55 + 45) / 90)
tabnext 4
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
nohlsearch
doautoall SessionLoadPost
unlet SessionLoad
" vim: set ft=vim :
