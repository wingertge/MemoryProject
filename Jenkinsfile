node {
  checkout scm

  def project = 'plenary-vim-176019'
  def imageTagBase = "eu.gcr.io/${project}"
  def entryName = "build.${BRANCH_NAME}.sh"
  def imageTagApi = "$imageTagBase/api:${BRANCH_NAME}.${BUILD_ID}"
  def imageTagWeb = "$imageTagBase/web:${BRANCH_NAME}.${BUILD_ID}"

  stage('Setup') {
    bat("gcloud container clusters get-credentials memoryproject-dev --zone europe-west1-c --project plenary-vim-176019")
  }

  stage('Webpack') {
	dir('MemoryClient.Web') {
	  retry(10) {
	    bat('npm install')
	  }
	  bat("webpack")
    }
  }

  stage('Build') {
    bat("docker build -t ${imageTagWeb} ./MemoryClient.Web")
	bat("docker build -t ${imageTagApi} ./MemoryServer")
  }

  stage('Deploy') {
    bat("docker build publish/api -t ${imageTagApi}")
    bat("docker build publish/web -t ${imageTagWeb}")
    bat("gcloud docker -- push ${imageTagApi}")
    bat("gcloud docker -- push ${imageTagWeb}")

	bat("sed -i.bak \"s#eu.gcr.io/plenary-vim-176019/api:1.0.0#${imageTagApi}#\" ./MemoryServer/k8s/${BRANCH_NAME}/*.yaml")
	bat("sed -i.bak \"s#eu.gcr.io/plenary-vim-176019/web:1.0.0#${imageTagWeb}#\" ./MemoryClient.Web/k8s/${BRANCH_NAME}/*.yaml")
    bat("kubectl --namespace=${BRANCH_NAME} apply -f MemoryServer/k8s/${BRANCH_NAME}/")
	bat("kubectl --namespace=${BRANCH_NAME} apply -f MemoryClient.Web/k8s/${BRANCH_NAME}/")
  }
}