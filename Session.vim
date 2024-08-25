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
badd +22 WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
badd +7 WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs
badd +10 WardrobeManager.Shared/Exceptions/Exceptions.cs
argglobal
%argdel
set stal=2
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
let s:l = 129 - ((13 * winheight(0) + 13) / 27)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 129
normal! 0
tabnext
edit WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
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
exe 'vert 1resize ' . ((&columns * 20 + 104) / 209)
exe 'vert 2resize ' . ((&columns * 188 + 104) / 209)
argglobal
enew
file NERD_tree_tab_2
balt WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
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
let s:l = 119 - ((31 * winheight(0) + 24) / 48)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 119
normal! 076|
wincmd w
exe 'vert 1resize ' . ((&columns * 20 + 104) / 209)
exe 'vert 2resize ' . ((&columns * 188 + 104) / 209)
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
exe '1resize ' . ((&lines * 12 + 25) / 51)
exe 'vert 1resize ' . ((&columns * 60 + 104) / 209)
exe '2resize ' . ((&lines * 35 + 25) / 51)
exe 'vert 2resize ' . ((&columns * 60 + 104) / 209)
exe '3resize ' . ((&lines * 20 + 25) / 51)
exe 'vert 3resize ' . ((&columns * 148 + 104) / 209)
exe '4resize ' . ((&lines * 27 + 25) / 51)
exe 'vert 4resize ' . ((&columns * 148 + 104) / 209)
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
let s:l = 12 - ((6 * winheight(0) + 6) / 12)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 12
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
let s:l = 44 - ((16 * winheight(0) + 17) / 35)
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
let s:l = 22 - ((8 * winheight(0) + 10) / 20)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 22
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
let s:l = 7 - ((5 * winheight(0) + 13) / 27)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 7
normal! 05|
wincmd w
exe '1resize ' . ((&lines * 12 + 25) / 51)
exe 'vert 1resize ' . ((&columns * 60 + 104) / 209)
exe '2resize ' . ((&lines * 35 + 25) / 51)
exe 'vert 2resize ' . ((&columns * 60 + 104) / 209)
exe '3resize ' . ((&lines * 20 + 25) / 51)
exe 'vert 3resize ' . ((&columns * 148 + 104) / 209)
exe '4resize ' . ((&lines * 27 + 25) / 51)
exe 'vert 4resize ' . ((&columns * 148 + 104) / 209)
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
let s:l = 24 - ((23 * winheight(0) + 24) / 48)
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
exe '1resize ' . ((&lines * 27 + 25) / 51)
exe 'vert 1resize ' . ((&columns * 23 + 104) / 209)
exe '2resize ' . ((&lines * 27 + 25) / 51)
exe 'vert 2resize ' . ((&columns * 96 + 104) / 209)
argglobal
enew
file NERD_tree_tab_1
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
let s:l = 10 - ((5 * winheight(0) + 13) / 27)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 10
normal! 0
wincmd w
exe '1resize ' . ((&lines * 27 + 25) / 51)
exe 'vert 1resize ' . ((&columns * 23 + 104) / 209)
exe '2resize ' . ((&lines * 27 + 25) / 51)
exe 'vert 2resize ' . ((&columns * 96 + 104) / 209)
tabnext 2
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
