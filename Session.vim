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
badd +0 WardrobeManager.Api/Program.cs
badd +7 WardrobeManager.Api/Endpoints/ClothingEndpoints.cs
badd +44 WardrobeManager.Api/Database/Services/Implementation/UserService.cs
badd +11 WardrobeManager.Api/Database/Services/Interfaces/IUserService.cs
badd +24 WardrobeManager.Shared/Services/Implementation/SharedService.cs
badd +23 WardrobeManager.Api/Endpoints/ImageEndpoints.cs
badd +47 WardrobeManager.Api/Database/Services/Implementation/FileService.cs
badd +9 WardrobeManager.Api/Database/Services/Interfaces/IFileService.cs
badd +23 WardrobeManager.Api/Endpoints/MiscEndpoints.cs
badd +22 WardrobeManager.Shared/Models/ServerClothingItem.cs
badd +2 WardrobeManager.Shared/Models/NewOrEditedClothingItem.cs
badd +12 WardrobeManager.Shared/Models/NewClothingItemDTO.cs
badd +2 WardrobeManager.Shared/Models/ClientClothingItem.cs
badd +5 WardrobeManager.Shared/Models/User.cs
badd +4 WardrobeManager.Shared/Enums/WearLocation.cs
badd +9 WardrobeManager.Shared/Services/Interfaces/ISharedService.cs
badd +1 WardrobeManager.Api/Endpoints/ActionEndpoints.cs
badd +22 WardrobeManager.Api/Database/Services/Implementation/ClothingItemService.cs
badd +7 WardrobeManager.Api/Database/Services/Interfaces/IClothingItemService.cs
badd +0 WardrobeManager.Shared/Exceptions/Exceptions.cs
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
let s:l = 129 - ((23 * winheight(0) + 24) / 48)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 129
normal! $
tabnext
edit WardrobeManager.Api/Endpoints/ClothingEndpoints.cs
argglobal
balt WardrobeManager.Shared/Models/ServerClothingItem.cs
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
let s:l = 14 - ((1 * winheight(0) + 25) / 51)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 14
normal! 05|
tabnext
edit WardrobeManager.Shared/Models/NewClothingItemDTO.cs
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
exe 'vert 1resize ' . ((&columns * 114 + 118) / 236)
exe 'vert 2resize ' . ((&columns * 121 + 118) / 236)
argglobal
balt WardrobeManager.Shared/Models/ServerClothingItem.cs
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
let s:l = 36 - ((35 * winheight(0) + 25) / 51)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 36
normal! 0
wincmd w
argglobal
if bufexists(fnamemodify("WardrobeManager.Shared/Models/ServerClothingItem.cs", ":p")) | buffer WardrobeManager.Shared/Models/ServerClothingItem.cs | else | edit WardrobeManager.Shared/Models/ServerClothingItem.cs | endif
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
let s:l = 55 - ((30 * winheight(0) + 25) / 51)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 55
normal! 0
wincmd w
exe 'vert 1resize ' . ((&columns * 114 + 118) / 236)
exe 'vert 2resize ' . ((&columns * 121 + 118) / 236)
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
exe '1resize ' . ((&lines * 25 + 27) / 54)
exe 'vert 1resize ' . ((&columns * 118 + 118) / 236)
exe '2resize ' . ((&lines * 25 + 27) / 54)
exe 'vert 2resize ' . ((&columns * 118 + 118) / 236)
exe '3resize ' . ((&lines * 40 + 27) / 54)
exe 'vert 3resize ' . ((&columns * 117 + 118) / 236)
exe '4resize ' . ((&lines * 10 + 27) / 54)
exe 'vert 4resize ' . ((&columns * 117 + 118) / 236)
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
let s:l = 7 - ((3 * winheight(0) + 12) / 25)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 7
normal! 04|
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
let s:l = 44 - ((12 * winheight(0) + 12) / 25)
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
let s:l = 22 - ((16 * winheight(0) + 20) / 40)
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
let s:l = 7 - ((2 * winheight(0) + 5) / 10)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 7
normal! 05|
wincmd w
exe '1resize ' . ((&lines * 25 + 27) / 54)
exe 'vert 1resize ' . ((&columns * 118 + 118) / 236)
exe '2resize ' . ((&lines * 25 + 27) / 54)
exe 'vert 2resize ' . ((&columns * 118 + 118) / 236)
exe '3resize ' . ((&lines * 40 + 27) / 54)
exe 'vert 3resize ' . ((&columns * 117 + 118) / 236)
exe '4resize ' . ((&lines * 10 + 27) / 54)
exe 'vert 4resize ' . ((&columns * 117 + 118) / 236)
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
exe '1resize ' . ((&lines * 48 + 27) / 54)
exe 'vert 1resize ' . ((&columns * 20 + 118) / 236)
exe '2resize ' . ((&lines * 48 + 27) / 54)
exe 'vert 2resize ' . ((&columns * 188 + 118) / 236)
argglobal
enew
file NERD_tree_tab_6
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
let s:l = 10 - ((9 * winheight(0) + 24) / 48)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 10
normal! 0
wincmd w
exe '1resize ' . ((&lines * 48 + 27) / 54)
exe 'vert 1resize ' . ((&columns * 20 + 118) / 236)
exe '2resize ' . ((&lines * 48 + 27) / 54)
exe 'vert 2resize ' . ((&columns * 188 + 118) / 236)
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
