FROM rust:1.67

# set up working directory
WORKDIR /app

# install third-party packages
RUN apt-get -y update && \
  apt-get install -y \
  libclang-dev \
  libssl-dev \
  openssl \
  pkg-config \
  protobuf-compiler

# copy the workspace source code
COPY . .

# compile postgrpc from source
RUN cargo build --release 

# use multi-stage build for smaller production image

# Run postgrpc 
CMD ["./target/release/postgrpc"]
