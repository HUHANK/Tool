@echo off
echo Fetch...
git fetch

echo Pull...
git pull

git remote update origin --prune

pause