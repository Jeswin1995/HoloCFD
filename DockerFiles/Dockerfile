# Base image
FROM opencfd/openfoam-default

# Set environment variables
ENV DEBIAN_FRONTEND noninteractive

# Install Node.js
RUN wget -qO- https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get update && apt-get install -y nodejs

# Install Node-RED
RUN npm install -g --unsafe-perm node-red

# Install additional Node-RED dependencies
RUN npm install -g --unsafe-perm node-red-dashboard
RUN npm install -g --unsafe-perm node-red-contrib-finite-statemachine
RUN npm install -g --unsafe-perm node-red-node-sqlite

# Install ParaView
RUN apt-get update && apt-get install -y paraview

# Source OpenFOAM environment in .bashrc
RUN echo "source /opt/openfoam10/etc/bashrc" >> /root/.bashrc

# Expose the Node-RED port
EXPOSE 1880

# Start Node-RED
CMD ["node-red"]
