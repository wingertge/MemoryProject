cd MemoryClient.Web
webpack
cd ..
docker run -i --rm -v "$pwd\:/sln" microsoft/aspnetcore-build:2.0.0-preview2 sh ./build.development.sh
docker build publish/api -t eu.gcr.io/plenary-vim-176019/api:dev
docker build publish/web -t eu.gcr.io/plenary-vim-176019/web:dev
gcloud docker -- push eu.gcr.io/plenary-vim-176019/api
gcloud docker -- push eu.gcr.io/plenary-vim-176019/web