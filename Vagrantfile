# -*- mode: ruby -*-
# vi: set ft=ruby :

# All Vagrant configuration is done below. The "2" in Vagrant.configure
# configures the configuration version (we support older styles for
# backwards compatibility). Please don't change it unless you know what
# you're doing.
Vagrant.configure(2) do |config|
  config.vm.box = "centos/7"

  config.vm.provision "shell", inline: <<-SHELL
    sudo yum install -y http://yum.postgresql.org/9.3/redhat/rhel-7-x86_64/pgdg-centos93-9.3-1.noarch.rpm
    sudo yum install -y postgresql93-server postgresql93-devel postgresql93-contrib
    sudo -H -u postgres bash -c '/usr/pgsql-9.3/bin/initdb -D /var/lib/pgsql/9.3/data'

    sudo cp /home/vagrant/sync/configs/pg_hba.conf /var/lib/pgsql/9.3/data/
    sudo chown postgres:postgres /var/lib/pgsql/9.3/data/pg_hba.conf
    sudo chmod 600 /var/lib/pgsql/9.3/data/pg_hba.conf

    sudo cp /home/vagrant/sync/configs/postgresql.conf /var/lib/pgsql/9.3/data/
    sudo chown postgres:postgres /var/lib/pgsql/9.3/data/postgresql.conf
    sudo chmod 600 /var/lib/pgsql/9.3/data/postgresql.conf

    sudo cp /home/vagrant/sync/configs/server.key /var/lib/pgsql/9.3/data/
    sudo chown postgres:postgres /var/lib/pgsql/9.3/data/server.key
    sudo chmod og-rwx /var/lib/pgsql/9.3/data/server.key

    sudo cp /home/vagrant/sync/configs/server.crt /var/lib/pgsql/9.3/data/
    sudo chown postgres:postgres /var/lib/pgsql/9.3/data/server.crt
    sudo chmod 600 /var/lib/pgsql/9.3/data/server.crt

    sudo systemctl start postgresql-9.3

    /usr/pgsql-9.3/bin/psql -U postgres -f /home/vagrant/sync/configs/setup.sql
    /usr/pgsql-9.3/bin/psql -U test -f /home/vagrant/sync/configs/create-table.sql

    sudo yum install -y httpd-tools
    sudo rpm --import /home/vagrant/sync/configs/mono-gpg-key
    sudo cp /home/vagrant/sync/configs/mono_project.repo /etc/yum.repos.d/mono_project.repo
    sudo yum install -y mono-complete-4.0.1-4

  SHELL
end

    # xbuild /home/vagrant/sync/ssl-begintransacton.sln
    # mono /home/vagrant/sync/ssl-begintransacton/bin/Debug/ssl-begintransacton.exe \&
    # sleep 10
    # ab -n 1000 -c 100 http://127.0.0.1:9832/query