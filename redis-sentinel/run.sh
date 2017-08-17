#!/bin/bash

# Copyright 2014 The Kubernetes Authors.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

MYIP=`getent hosts ${MY_POD_IP} | awk '{ print $1 }'`

sed -i "s/\$SENTINEL_QUORUM/$SENTINEL_QUORUM/g" /redis.conf
sed -i "s/\$SENTINEL_DOWN_AFTER/$SENTINEL_DOWN_AFTER/g" /redis.conf
sed -i "s/\$SENTINEL_FAILOVER/$SENTINEL_FAILOVER/g" /redis.conf
sed -i "s/\$MY_IP/$MY_IP/g" /redis.conf

if [[ ${GET_HOSTS_FROM:-dns} == "env" ]]; then
  sed -i "s/\$MASTER_HOST/$REDIS_MASTER_SERVICE_HOST/g" /redis.conf

  redis-server --slaveof ${REDIS_MASTER_SERVICE_HOST} 6379
else
  sed -i "s/\$MASTER_HOST/redis-master/g" /redis.conf

  redis-server /redis.conf --sentinel
fi