cd MemoryClient.Web
webpack
cd ..
docker-compose -f .\docker-compose.yml up
docker build publish/api -t eu.gcr.io/plenary-vim-176019/api
docker build publish/web -t eu.gcr.io/plenary-vim-176019/web
gcloud docker -- push eu.gcr.io/plenary-vim-176019/api
gcloud docker -- push eu.gcr.io/plenary-vim-176019/web