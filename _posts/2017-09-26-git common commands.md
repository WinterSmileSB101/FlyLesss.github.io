---
layout: post
title: git知识点及常用命令
date: 2017-09-26 15:08:24.000000000 +09:00
categories: [git]
tag: git
---
本地仓库名要和远程仓库名一致 否则pull不下来
创建本地仓库
比如  `Amber.Li`<br>
`git init`   初始化本地仓库   创建隐藏文件 `./git`<br>
`git remote -v`  查看远程仓库链接地址  <br>
`git remote add origin`(这个随意  自己取  以后就用这个名称代替远程仓库)  SSH地址<br>
`git pull origin master`(远程仓库master分支)   先从远程仓库pull到本地   在进行上传操作<br>
在本地添加要上传的文件<br>
`git add .`<br>
`git commit -m "输入提交的提示信息"`<br>
`git status`  可以查看缓冲池是干净的<br>
`git push origin master`<br>

* 目录
{:toc}
## Git常用命令<br>
查看、添加、提交、删除、找回，重置修改文件<br>
`git help <command>` 显示command的help<br>
`git show `# 显示某次提交的内容 git show $id<br>
`git co -- <file>` # 抛弃工作区修改<br>
`git co .` # 抛弃工作区修改<br>
`git add <file> `# 将工作文件修改提交到本地暂存区<br>
`git add . `# 将所有修改过的工作文件提交暂存区<br>
`git rm <file>` # 从版本库中删除文件<br>
`git rm <file> --cached `# 从版本库中删除文件，但不删除文件<br>
`git reset <file>` # 从暂存区恢复到工作文件<br>
`git reset -- .` # 从暂存区恢复到工作文件<br>
`git reset --hard` # 恢复最近一次提交过的状态，即放弃上次提交后的所有本次修改<br>
`git ci <file> git ci . git ci -a `# 将git add, git rm和git ci等操作都合并在一起做
`git ci -am "some comments"`<br>
`git ci --amend` # 修改最后一次提交记录<br>
`git revert <$id>` # 恢复某次提交的状态，恢复动作本身也创建次提交对象<br>
`git revert HEAD` # 恢复最后一次提交的状态<br>

查看文件diff<br>
`git diff <file>` # 比较当前文件和暂存区文件差异 git diff<br>
`git diff <id1><id1><id2>` # 比较两次提交之间的差异<br>
`git diff <branch1>..<branch2>` # 在两个分支之间比较<br>
`git diff --staged` # 比较暂存区和版本库差异<br>
`git diff --cached` # 比较暂存区和版本库差异<br>
`git diff --stat` # 仅仅比较统计信息<br>

查看提交记录<br>
`git log git log` <file> # 查看该文件每次提交记录<br>
`git log -p` <file> # 查看每次详细修改内容的diff<br>
`git log -p -2` # 查看最近两次详细修改内容的diff<br>
`git log --stat `#查看提交统计信息<br>

tig<br>

Mac上可以使用tig代替diff和log，brew install tig<br>

Git 本地分支管理<br>

查看、切换、创建和删除分支<br>
`git br -r` # 查看远程分支<br>
`git br <new_branch>` # 创建新的分支<br>
`git br -v` # 查看各个分支最后提交信息<br>
`git br --merged` # 查看已经被合并到当前分支的分支<br>
`git br --no-merged` # 查看尚未被合并到当前分支的分支<br>
`git co <branch>` # 切换到某个分支<br>
`git co -b <new_branch>` # 创建新的分支，并且切换过去<br>
`git co -b <new_branch> <branch>` # 基于branch创建新的new_branch<br>
`git co $id` # 把某次历史提交记录checkout出来，但无分支信息，切换到其他分支会自动删除<br>
`git co $id -b <new_branch>` # 把某次历史提交记录checkout出来，创建成一个分支<br>
`git br -d <branch>` # 删除某个分支<br>
`git br -D <branch>` # 强制删除某个分支 (未被合并的分支被删除的时候需要强制)<br>

分支合并和rebase<br>
`git merge <branch>` # 将branch分支合并到当前分支<br>
`git merge origin/master --no-ff` # 不要Fast-Foward合并，这样可以生成merge提交<br>
`git rebase master <branch>` # 将master rebase到branch，相当于： `git co <branch> && git rebase master && git co master && git merge <branch>`<br>

Git补丁管理(方便在多台机器上开发同步时用)<br>
`git diff > ../sync.patch` # 生成补丁<br>
`git apply ../sync.patch `# 打补丁<br>
`git apply --check ../sync.patch` #测试补丁能否成功<br>

Git暂存管理<br>
`git stash` # 暂存<br>
`git stash list` # 列所有stash<br>
`git stash apply` # 恢复暂存的内容<br>
`git stash drop` # 删除暂存区<br>

Git远程分支管理<br>
`git pull` # 抓取远程仓库所有分支更新并合并到本地<br>
`git pull --no-ff` # 抓取远程仓库所有分支更新并合并到本地，不要快进合并<br>
`git fetch origin `# 抓取远程仓库更新<br>
`git merge origin/master` # 将远程主分支合并到本地当前分支<br>
`git co --track origin/branch` # 跟踪某个远程分支创建相应的本地分支<br>
`git co -b <local_branch> origin/<remote_branch>` # 基于远程分支创建本地分支，功能同上<br>

`git push `# push所有分支<br>
`git push origin master` # 将本地主分支推到远程主分支<br>
`git push -u origin master` # 将本地主分支推到远程(如无远程主分支则创建，用于初始化远程仓库)<br>
`git push origin` <local_branch> # 创建远程分支， origin是远程仓库名<br>
`git push origin` <local_branch>:<remote_branch> # 创建远程分支<br>
`git push origin :<remote_branch>` #先删除本地分支(git br -d <branch>)，然后再push删除远程分支<br>

Git远程仓库管理<br>

GitHub<br>
`git remote -v `# 查看远程服务器地址和仓库名称<br>
`git remote show origin` # 查看远程服务器仓库状态<br>
`git remote add origin git@ github:robbin/robbin_site.git` # 添加远程仓库地址<br>
`git remote set-url origin git@ github.com:robbin/robbin_site.git` # 设置远程仓库地址(用于修改远程仓库地址)<br>
`git remote rm <repository>` # 删除远程仓库<br>
创建远程仓库<br>
`git clone --bare robbin_site robbin_site.git` # 用带版本的项目创建纯版本仓库<br>
`scp -r my_project.git git@ git.csdn.net:~ `# 将纯仓库上传到服务器上<br>
`mkdir robbin_site.git && cd robbin_site.git && git --bare init `# 在服务器创建纯仓库<br>
`git remote add origin git@ github.com:robbin/robbin_site.git` # 设置远程仓库地址<br>
`git push -u origin master` # 客户端首次提交<br>
`git push -u origin develop`# 首次将本地develop分支提交到远程develop分支，并且track<br>
`git remote set-head origin master` # 设置远程仓库的HEAD指向master分支
也可以命令设置跟踪远程库和本地库<br>
`git branch --set-upstream master origin/master`<br>
`git branch --set-upstream develop origin/develop`<br>


